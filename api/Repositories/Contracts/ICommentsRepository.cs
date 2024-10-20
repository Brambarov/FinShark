using api.Models;

namespace api.Repositories.Contracts
{
    public interface ICommentsRepository : IModelRepository
    {
        Task<Comment> CreateAsync(Comment model);
        Task<Comment?> UpdateAsync(int id, Comment updatedModel);
    }
}