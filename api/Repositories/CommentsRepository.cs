using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.Repositories.Contracts;
using api.Models;
using api.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IModel>> GetAllAsync()
        {
            return new List<IModel>(await _context.Comments.ToListAsync());
        }

        public async Task<IModel?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment model)
        {
            await _context.Comments.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<Comment?> UpdateAsync(int id, PutCommentDTO DTO)
        {
            var model = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return null;
            }

            model.Title = DTO.Title;
            model.Content = DTO.Content;

            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<IModel?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}