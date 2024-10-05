using BOs;
using BOs.DTOS;
using DAOs;
using Microsoft.AspNetCore.Mvc;
using Repos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : Controller
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
        {
            var areas = _areaService.GetAreas();
            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Area>> GetArea(Guid id)
        {
            var area = _areaService.GetArea(id);
            if (area == null)
            {
                return NotFound("Area not found.");
            }

            return Ok(area);
        }

        [HttpPost]
        public async Task<ActionResult> CreateArea([FromBody] AreaCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var area = _areaService.CreateArea(request);
            return CreatedAtAction(nameof(GetArea), new { id = area.AreaID }, area);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(Guid id, [FromBody] AreaUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedArea = _areaService.UpdateArea(id, request);
            if (updatedArea == null)
            {
                return NotFound();
            }

            return Ok(updatedArea);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(Guid id)
        {
            var isDeleted = _areaService.DeleteArea(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
