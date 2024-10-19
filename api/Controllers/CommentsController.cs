using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Repositories.Contracts;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.DTOs.Comment;

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
            var models = (await _commentsRepository.GetAllAsync()).Select(m => m as Comment);

            var DTOs = models.Select(c => c?.ToGetDTO());

            return Ok(DTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var model = await _commentsRepository.GetByIdAsync(id) as Comment;

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model.ToGetDTO());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, PostCommentDTO DTO)
        {
            if (!await _stocksRepository.ExistsAsync(stockId))
            {
                return BadRequest("Stock does not exist!");
            }

            var model = DTO.FromPostDTO(stockId);

            await _commentsRepository.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model.ToGetDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PutCommentDTO DTO)
        {
            var model = await _commentsRepository.UpdateAsync(id, DTO);

            if(model == null)
            {
                return NotFound();
            }

            return Ok(model.ToGetDTO());
        }
    }
}