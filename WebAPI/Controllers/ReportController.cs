using BOs.DTOS;
using Microsoft.AspNetCore.Mvc;
using Repos;

namespace WebAPI.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // 3. Create Report
        [HttpPost("report")]
        public IActionResult CreateReport([FromBody] CreateReportRequest request)
        {
            try
            {
                _reportService.CreateReport(request);
                return Ok(new { message = "Report created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
