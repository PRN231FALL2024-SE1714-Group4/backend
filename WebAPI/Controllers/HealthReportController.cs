using BOs;
using BOs.DTOS;
using Microsoft.AspNetCore.Mvc;
using Repos;
using WebAPI.Filter;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthReportController : ControllerBase
    {
        private readonly IHealthReportService _healthReportService;

        public HealthReportController(IHealthReportService healthReportService)
        {
            _healthReportService = healthReportService;
        }

        // GET: api/HealthReport
        [HttpGet]
        public ActionResult<IEnumerable<HealthReport>> GetAllHealthReports()
        {
            try
            {
                var healthReports = _healthReportService.GetAllHealthReports();
                return Ok(healthReports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/HealthReport/{id}
        [HttpGet("{id}")]
        public ActionResult<HealthReport> GetHealthReportById(Guid id)
        {
            try
            {
                var healthReport = _healthReportService.GetHealthReportById(id);
                if (healthReport == null)
                {
                    return NotFound("Health report not found.");
                }

                return Ok(healthReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/HealthReport
        [HttpPost]
        [JwtAuthorize("STAFF")]
        public ActionResult AddHealthReport([FromBody] HealthReportCreateRequest healthReportCreate)
        {
            try
            {
                if (healthReportCreate == null)
                {
                    return BadRequest("Invalid data.");
                }

                _healthReportService.AddHealthReport(healthReportCreate);
                return Ok("Health report added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/HealthReport/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateHealthReport(Guid id, [FromBody] HealthReportUpdateRequest healthReport)
        {
            try
            {
                if (healthReport == null || id == Guid.Empty)
                {
                    return BadRequest("Invalid data.");
                }

                bool isUpdated = _healthReportService.UpdateHealthReport(id, healthReport);
                if (!isUpdated)
                {
                    return NotFound("Health report not found or could not be updated.");
                }

                return Ok("Health report updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/HealthReport/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteHealthReport(Guid id)
        {
            try
            {
                bool isDeleted = _healthReportService.DeleteHealthReport(id);
                if (!isDeleted)
                {
                    return NotFound("Health report not found or could not be deleted.");
                }

                return Ok("Health report deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
