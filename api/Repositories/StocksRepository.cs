using api.Data;
using api.Helpers;
using api.Models;
using api.Models.Contracts;
using api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;

        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ModelBase>> GetAllAsync(QueryParameters? queryParameters)
        {
            var stocks = _context.Stocks
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters?.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(queryParameters.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(queryParameters?.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(queryParameters.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(queryParameters?.SortBy))
            {
                if (queryParameters.SortBy.Equals("symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryParameters.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (queryParameters.PageNumber - 1) * queryParameters.PageSize;

            return await stocks
                .Select(s => s as ModelBase)
                .Skip(skipNumber)
                .Take(queryParameters.PageSize)
                .ToListAsync();
        }

        public async Task<ModelBase?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<Stock> CreateAsync(Stock model)
        {
            await _context.Stocks.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<Stock?> UpdateAsync(int id, Stock updatedModel)
        {
            var existingModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingModel == null)
            {
                return null;
            }

            existingModel.Symbol = updatedModel.Symbol;
            existingModel.CompanyName = updatedModel.CompanyName;
            existingModel.Purchase = updatedModel.Purchase;
            existingModel.LastDiv = updatedModel.LastDiv;
            existingModel.Industry = updatedModel.Industry;
            existingModel.MarketCap = updatedModel.MarketCap;

            await _context.SaveChangesAsync();

            return existingModel;
        }

        public async Task<ModelBase?> DeleteAsync(int id)
        {
            var model = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return null;
            }

            _context.Stocks.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}