using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static GetCommentDTO ToGetDTO(this Comment model)
        {
            return new GetCommentDTO
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                CreatedOn = model.CreatedOn,
                StockId = model.StockId
            };
        }

        public static Comment FromPostDTO(this PostCommentDTO DTO, int stockId)
        {
            return new Comment
            {
                Title = DTO.Title,
                Content = DTO.Content,
                StockId = stockId
            };
        }
    }
}