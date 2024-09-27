﻿using BOs;
using DAOs;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AreaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
        {
            var areas = _unitOfWork.AreaRepository.Get();
            return Ok(areas);
        }

        // GET: api/area/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Area>> GetArea(Guid id)
        {
            var area = _unitOfWork.AreaRepository.GetByID(id);

            if (area == null)
            {
                return NotFound();
            }

            return Ok(area);
        }

        // POST: api/area
        [HttpPost]
        public async Task<ActionResult> CreateArea([FromBody] AreaCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var area = new Area
            {
                Name = request.Name
            };

            _unitOfWork.AreaRepository.Insert(area);
            _unitOfWork.Save();

            return Ok(CreatedAtAction(nameof(GetArea), new { id = area.AreaID }, area));
        }

        // PUT: api/area/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(Guid id, [FromBody] AreaUpdateController request)
        {
            if (id != request.AreaID)
            {
                return BadRequest("ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var areaToUpdate = _unitOfWork.AreaRepository.GetByID(id);
            if (areaToUpdate == null)
            {
                return NotFound();
            }

            areaToUpdate.Name = request.Name;

            _unitOfWork.AreaRepository.Update(id, areaToUpdate);
            _unitOfWork.Save();

            return Ok();
        }

        // DELETE: api/area/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(Guid id)
        {
            var isDeleted = _unitOfWork.AreaRepository.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            _unitOfWork.Save();
            return Ok();
        }
    }
}
