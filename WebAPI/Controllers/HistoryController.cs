using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BOs;
using BOs.DTOS;
using Repos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : Controller
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        // GET: api/History
        [HttpGet]
        public async Task<IActionResult> GetAllHistories()
        {
            var histories = await _historyService.GetAllHistoriesAsync();
            return Ok(histories);
        }

        // GET: api/History/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistoryById(Guid id)
        {
            var history = await _historyService.GetHistoryByIdAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            return Ok(history);
        }

        // POST: api/History
        [HttpPost]
        public async Task<IActionResult> CreateHistory([FromBody] CreateHistoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdHistory = await _historyService.CreateHistoryAsync(request);
            return CreatedAtAction(nameof(GetHistoryById), new { id = createdHistory.HistoryID }, createdHistory);
        }

        // PUT: api/History/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateHistory(Guid id, [FromBody] CreateHistoryRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var updatedHistory = await _historyService.UpdateHistoryAsync(id, request);
        //    if (updatedHistory == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(updatedHistory);
        //}

        // DELETE: api/History/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteHistory(Guid id)
        //{
        //    var success = await _historyService.DeleteHistoryAsync(id);
        //    if (!success)
        //    {
        //        return NotFound();
        //    }

        //    return OK(success);
        //}
    }
}
