using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Repositories.Contracts
{
    public interface ICommentsRepository : IModelRepository
    {
        Task<Comment> CreateAsync(Comment model);
        Task<Comment?> UpdateAsync (int id, Comment updatedModel);
    }
}