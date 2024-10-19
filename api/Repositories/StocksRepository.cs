using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Repositories.Contracts;
using api.Data;
using api.DTOs.Stock;
using api.Models;
using api.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;

        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IModel>> GetAllAsync()
        {
            return new List<IModel>(await _context.Stocks.Include(s => s.Comments).ToListAsync());
        }

        public async Task<IModel?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
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

        public async Task<IModel?> DeleteAsync(int id)
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