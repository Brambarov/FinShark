using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksRepository _stockRepository;

        public StocksController(IStocksRepository stocksRepository)
        {
            _stockRepository = stocksRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stockModels = await _stockRepository.GetAllAsync();

            var stockDTOs = stockModels.Select(x => x.ToGetStockDTO());

            return Ok(stockDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stockModel = await _stockRepository.GetByIdAsync(id);

            if(stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToGetStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostStockDTO stockDTO)
        {
            var stockModel = stockDTO.ToStockFromPostStockDTO();

            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToGetStockDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PutStockDTO stockDTO)
        {
            var stockModel = await _stockRepository.UpdateAsync(id, stockDTO);

            if(stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToGetStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepository.DeleteAsync(id);

            if(stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}