using BOs;
using BOs.DTOS;
using BOs.Enum;
using Microsoft.AspNetCore.Mvc;
using Repos;
using Repos.Response;
using WebAPI.Filter;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserShiftController : Controller
    {
        private readonly IUserShiftService _userShiftService;

        // Inject the IUserShiftService through the constructor
        public UserShiftController(IUserShiftService userShiftService)
        {
            _userShiftService = userShiftService;
        }

        [HttpPost]
        [JwtAuthorize("MANAGER", "STAFF")]
        public IActionResult CreateUserShift([FromBody] List<UserShiftRequest> userShiftRequests)
        {
            if (userShiftRequests == null || !userShiftRequests.Any())
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                bool result = _userShiftService.createUserShift(userShiftRequests);

                if (result)
                {
                    return Ok("User shifts created successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while creating user shifts.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("workers-in-shift")]
        [JwtAuthorize("MANAGER", "STAFF")]
        public ActionResult<List<WorkerInShiftResponse>> GetWorkersInShift([FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate)
        {
            // Check if the date range is valid
            if (fromDate > toDate)
            {
                return BadRequest("Invalid date range.");
            }

            try
            {
                // Call the service to get the workers in shift
                var workersInShift = _userShiftService.getAllWorkerInShift(fromDate, toDate);

                // Check if the result is empty
                if (workersInShift == null || !workersInShift.Any())
                {
                    return NotFound("No shifts found for the specified date range.");
                }

                return Ok(workersInShift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("available-users")]
        [JwtAuthorize("MANAGER", "STAFF")]
        public ActionResult<List<User>> getUser([FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate, WorkShiftEnum workShiftEnum)
        {
            // Check if the date range is valid
            if (fromDate > toDate)
            {
                return BadRequest("Invalid date range.");
            }

            try
            {
                // Call the service to get the workers in shift
                var workersInShift = _userShiftService.getAvailableUserForSpecificShift(fromDate, toDate, workShiftEnum);

                // Check if the result is empty
                if (workersInShift == null || !workersInShift.Any())
                {
                    return NotFound("No shifts found for the specified date range.");
                }

                return Ok(workersInShift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("me")]
        [JwtAuthorize("MANAGER", "STAFF")]
        public ActionResult<List<UserShift>> getMyShift([FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate)
        {
            // Check if the date range is valid
            if (fromDate > toDate)
            {
                return BadRequest("Invalid date range.");
            }

            try
            {
                // Call the service method to get the current user's shifts
                var userShifts = _userShiftService.getMyShift(fromDate, toDate);

                // Check if any shifts were found for the current user
                if (userShifts == null || !userShifts.Any())
                {
                    return NotFound("No shifts found for the specified date range.");
                }

                // Return the list of user shifts
                return Ok(userShifts);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
