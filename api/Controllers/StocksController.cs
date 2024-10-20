using api.DTOs.Stock;
using api.Helpers;
using api.Mappers;
using api.Models;
using api.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksRepository _stockRepository;

        public StocksController(IStocksRepository stocksRepository)
        {
            _stockRepository = stocksRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var models = (await _stockRepository.GetAllAsync(queryParameters)).Select(m => m as Stock);

            var DTOs = models
                .Select(s => s.ToGetDTO())
                .ToList();

            return Ok(DTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _stockRepository.GetByIdAsync(id) as Stock;

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model.ToGetDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostStockDTO stockDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = stockDTO.FromPostDTO();

            await _stockRepository.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model.ToGetDTO());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PutStockDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _stockRepository.UpdateAsync(id, DTO.ToModel());

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model.ToGetDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _stockRepository.DeleteAsync(id) as Stock;

            if (model == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}