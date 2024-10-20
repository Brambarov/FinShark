using api.Helpers;
using api.Models.Contracts;

namespace api.Repositories.Contracts
{
    public interface IModelRepository
    {
        Task<List<ModelBase>> GetAllAsync(QueryParameters? queryParameters);
        Task<ModelBase?> GetByIdAsync(int id);
        Task<ModelBase?> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}