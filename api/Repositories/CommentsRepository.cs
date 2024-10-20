using api.Data;
using api.Helpers;
using api.Models;
using api.Models.Contracts;
using api.Repositories.Contracts;
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

        public async Task<List<ModelBase>> GetAllAsync(QueryParameters? queryParameters)
        {
            return new List<ModelBase>(await _context.Comments.Include(c => c.User).ToListAsync());
        }

        public async Task<ModelBase?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment model)
        {
            await _context.Comments.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment updatedModel)
        {
            var existingModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (existingModel == null)
            {
                return null;
            }

            existingModel.Title = updatedModel.Title;
            existingModel.Content = updatedModel.Content;

            await _context.SaveChangesAsync();

            return existingModel;
        }

        public async Task<ModelBase?> DeleteAsync(int id)
        {
            var model = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null)
            {
                return null;
            }

            _context.Comments.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}