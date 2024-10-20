using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Repositories.Contracts;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.DTOs.Comment;
using api.Helpers;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ICommentsRepository _commentsRepository;

        public CommentsController(IStocksRepository stocksRepository, ICommentsRepository commentsRepository)
        {
            _stocksRepository = stocksRepository;
            _commentsRepository = commentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var models = (await _commentsRepository.GetAllAsync(null)).Select(m => m as Comment);

            var DTOs = models.Select(c => c?.ToGetDTO());

            return Ok(DTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _commentsRepository.GetByIdAsync(id) as Comment;

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model.ToGetDTO());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, PostCommentDTO DTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stocksRepository.ExistsAsync(stockId))
            {
                return BadRequest("Stock does not exist!");
            }

            var model = DTO.ToModel(stockId);

            await _commentsRepository.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model.ToGetDTO());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PutCommentDTO DTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _commentsRepository.UpdateAsync(id, DTO.ToModel());

            if(model == null)
            {
                return NotFound();
            }

            return Ok(model.ToGetDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _commentsRepository.DeleteAsync(id) as Comment;

            if(model == null)
            {
                return NotFound("Comment does not exist!");
            }

            return NoContent();
        }
    }
}