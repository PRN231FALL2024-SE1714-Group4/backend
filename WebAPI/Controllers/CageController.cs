using BOs;
using BOs.DTOS;
using DAOs;
using Microsoft.AspNetCore.Mvc;
using Repos;
using WebAPI.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CageController : Controller
    {
        private readonly ICageService _cageService;

        public CageController(ICageService cageService)
        {
            _cageService = cageService;
        }

        // GET: api/cage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cage>>> GetCages()
        {
            var cages = await _cageService.GetCages();
            return Ok(cages);
        }

        // GET: api/cage/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cage>> GetCage(Guid id)
        {
            var cage = await _cageService.GetCageById(id);
            return cage != null ? Ok(cage) : NotFound("Cage ID does not exist.");
        }

        [HttpGet("area/{id}")]
        public async Task<ActionResult<List<Cage>>> GetCagesOfArea(Guid id)
        {
            var cages = await _cageService.GetCagesByAreaId(id);
            return cages.Any() ? Ok(cages) : NotFound("No cages found for the specified Area ID.");
        }

        // POST: api/cage
        [HttpPost]
        public async Task<ActionResult> CreateCage([FromBody] CageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cage = await _cageService.CreateCage(request);
            return Ok(CreatedAtAction(nameof(GetCage), new { id = cage.CageID }, cage));
        }

        // PUT: api/cage/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCage(Guid id, [FromBody] CageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _cageService.UpdateCage(id, request);
            return updated ? Ok() : NotFound("Cage not found.");
        }

        // DELETE: api/cage/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCage(Guid id)
        {
            var isDeleted = await _cageService.DeleteCage(id);
            return isDeleted ? Ok() : NotFound();
        }

        [HttpGet("get-animal-in-cage/{id}")]
        public async Task<ActionResult<List<Animal>>> GetAnimalsInCage(Guid id)
        {
            var cages = _cageService.GetAnimalsInCage(id);
            return cages != null ? Ok(cages) : Ok("No animal found for the specified Area ID.");
        }
    }
}
