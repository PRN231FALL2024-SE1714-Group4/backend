using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repos;
using Repos.Response;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : Controller
    {
        private readonly IUserService _userService;

        public GoogleController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleResponse)),
            };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<ActionResult<UserResponse>> GoogleResponse()
        {
            try
            {
                var response = await _userService.VerifyGoogleSignIn();
                return Ok(response);  // Return the user details along with the token
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
