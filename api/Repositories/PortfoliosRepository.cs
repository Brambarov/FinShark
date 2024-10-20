using api.Data;
using api.Models;
using api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfoliosRepository : IPortfoliosRepository
    {
        private readonly ApplicationDbContext _context;

        public PortfoliosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetPortfolioByUser(User user)
        {
            return await _context.Portfolios.Where(p => p.UserId == user.Id)
                .Select(p => new Stock
                {
                    Id = p.Stock.Id,
                    Symbol = p.Stock.Symbol,
                    CompanyName = p.Stock.CompanyName,
                    Purchase = p.Stock.Purchase,
                    LastDiv = p.Stock.LastDiv,
                    Industry = p.Stock.Industry,
                    MarketCap = p.Stock.MarketCap
                }).ToListAsync();
        }
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio> DeleteAsync(User user, string symbol)
        {
            var model = await _context.Portfolios.FirstOrDefaultAsync(p => p.UserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());

            if (model == null)
            {
                return null;
            }

            _context.Portfolios.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}
