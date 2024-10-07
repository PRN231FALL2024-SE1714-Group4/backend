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
        [HttpPost("/create")]
        public IActionResult CreateReport([FromBody] CreateReportRequest request)
        {
            try
            {
                var report = _reportService.CreateReport(request);
                return CreatedAtAction(nameof(GetReportById), new { id = report.ReportID }, report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 2. PUT: api/reports/{id} (Update an existing report)
        [HttpPut("{id}")]
        public IActionResult UpdateReport(Guid id, [FromBody] CreateReportRequest request)
        {
            try
            {
                var report = _reportService.UpdateReport(id, request);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 3. GET: api/reports/{id} (Get report by ID)
        [HttpGet("{id}")]
        public IActionResult GetReportById(Guid id)
        {
            try
            {
                var report = _reportService.GetReportById(id);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // 4. GET: api/reports (Get all reports or filtered by workId)
        [HttpGet]
        public IActionResult GetReports([FromQuery] Guid? workId)
        {
            var reports = _reportService.GetReports(workId);
            return Ok(reports);
        }

        // 5. DELETE: api/reports/{id} (Delete report by ID)
        [HttpDelete("{id}")]
        public IActionResult DeleteReport(Guid id)
        {
            try
            {
                _reportService.DeleteReport(id);
                return Ok(new { Message = "Report deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
