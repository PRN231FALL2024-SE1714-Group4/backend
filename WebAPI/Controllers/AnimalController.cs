using BOs.DTOS;
using BOs;
using Microsoft.AspNetCore.Mvc;
using Repos;
using Repos.Implements;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : Controller
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        // GET: api/Animal
        [HttpGet]
        public ActionResult<List<Animal>> GetAnimals()
        {
            var animals = _animalService.GetAnimals();
            return Ok(animals);
        }

        // GET: api/Animal/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimalById(Guid id)
        {
            var animal = await _animalService.GetAnimalById(id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }

        // POST: api/Animal
        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal([FromBody] AnimalCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAnimal = await _animalService.CreateAnimal(request);
            return Ok(createdAnimal);
        }

        [HttpPut("{id}")]
        public ActionResult<Animal> UpdateAnimal(Guid id, [FromBody] AnimalUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateAnimal = _animalService.UpdateAnimal(id, request);
            if (updateAnimal == null)
            {
                return NotFound();
            }

            return Ok(updateAnimal);
        }
    }
}
