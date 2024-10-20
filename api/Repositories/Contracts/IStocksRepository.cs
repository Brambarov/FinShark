using api.Models;

namespace api.Repositories.Contracts
{
    public interface IStocksRepository : IModelRepository
    {
        Task<Stock> CreateAsync(Stock model);
        Task<Stock?> UpdateAsync(int id, Stock updatedModel);

    }
}