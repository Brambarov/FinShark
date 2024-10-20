using api.Helpers;
using api.Models.Contracts;

namespace api.Repositories.Contracts
{
    public interface IModelRepository
    {
        Task<List<IModel>> GetAllAsync(QueryParameters? queryParameters);
        Task<IModel?> GetByIdAsync(int id);
        Task<IModel?> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}