using BOs;
using DAOs;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/cage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cage>>> GetCages()
        {
            var cages = _unitOfWork.CageRepository.Get();
            return Ok(cages);
        }

        // GET: api/cage/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cage>> GetCage(Guid id)
        {
            try
            {
                var cage = _unitOfWork.CageRepository
                    .Get(   filter: c => c.CageID == id, 
                            includeProperties: "Area,Histories")
                    .FirstOrDefault();
                return cage != null ? Ok(cage) : NotFound("Cage ID does not exist.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("area/{id}")]
        public async Task<ActionResult<List<Cage>>> GetCageOfArea(Guid id)
        {
            try
            {
                var cages = _unitOfWork.CageRepository
                    .Get(filter: p => p.AreaID == id, 
                    includeProperties: "Area")
                    .ToList();

                return cages.Any() ? Ok(cages) : NotFound("No cages found for the specified Area ID.");
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }


        // POST: api/cage
        [HttpPost]
        public async Task<ActionResult> CreateCage([FromBody] CageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cage = new Cage
            {
                CageName = request.CageName,
                AreaID = request.AreaID
            };

            _unitOfWork.CageRepository.Insert(cage);
            _unitOfWork.Save();

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

            var cageToUpdate = _unitOfWork.CageRepository.GetByID(id);
            if (cageToUpdate == null)
            {
                return NotFound();
            }

            cageToUpdate.AreaID = request.AreaID;

            _unitOfWork.CageRepository.Update(id, cageToUpdate);
            _unitOfWork.Save();

            return Ok();
        }

        // DELETE: api/cage/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCage(Guid id)
        {
            var isDeleted = _unitOfWork.CageRepository.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            _unitOfWork.Save();
            return Ok();
        }
    }
}
