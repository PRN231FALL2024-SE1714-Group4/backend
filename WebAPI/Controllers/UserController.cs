using BOs.DTOS;
using Microsoft.AspNetCore.Mvc;
using Repos.Response;
using Repos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public ActionResult<UserResponse> Register(UserRegisterRequest request)
        {
            try
            {
                // Call the registerAccount method from the service
                UserResponse userResponse = _userService.registerAccount(request);
                return Ok(new
                {
                    Token = userResponse.Token,
                    FullName = userResponse.FullName,
                    Email = userResponse.Email,
                    Role = userResponse.Role
                }); // Return 200 OK with the user details
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return 400 Bad Request if something goes wrong
            }
        }

        [HttpPost("login")]
        public ActionResult<UserResponse> Login(UserLoginRequest request)
        {
            try
            {
                var userResponse = _userService.Login(request);
                return Ok(new
                {
                    Token = userResponse.Token,
                    FullName = userResponse.FullName,
                    Email = userResponse.Email,
                    Role = userResponse.Role
                }); // Return 200 OK with the user details
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return 400 Bad Request if something goes wrong
            }
        }
    }
}
