using BOs;
using BOs.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class WorkController : Controller
    {
        private readonly IWorkService _workservice;

        public WorkController(IWorkService workService)
        {
            _workservice = workService;
        }

        // 1. Create Work (Assign Task)
        [HttpPost("create")]
        public ActionResult<Work> CreateWork([FromBody] CreateWorkRequest request)
        {
            try
            {
                var work = _workservice.CreateWork(request);
                return Ok(work);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Work> GetWork(Guid id)
        {
            try
            {
                var work = _workservice.GetWorkByID(id);
                return Ok(work);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. View My Work (Assignee)
        [HttpGet("my-work")]
        public IActionResult ViewMyWork(Guid id)
        {
            try
            {
                var tasks = _workservice.ViewMyWork();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //// 3. Create Report
        //[HttpPost("report")]
        //public IActionResult CreateReport([FromBody] CreateReportRequest request)
        //{
        //    try
        //    {
        //        _workservice.CreateReport(request);
        //        return Ok(new { message = "Report created successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        // 4. View Assigned Tasks (Assigner)
        [HttpGet("assigned-tasks")]
        public IActionResult ViewAssignedTasks()
        {
            try
            {
                var tasks = _workservice.ViewAssignedTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
