using api.Models;

namespace api.Repositories.Contracts
{
    public interface IPortfoliosRepository
    {
        Task<List<Stock>> GetPortfolioByUser(User user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeleteAsync(User user, string symbol);
    }
}
