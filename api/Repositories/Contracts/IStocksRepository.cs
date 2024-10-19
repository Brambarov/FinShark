using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Repositories.Contracts;
using api.Models;

namespace api.Repositories.Contracts
{
    public interface IStocksRepository : IModelRepository
    {
        Task<Stock> CreateAsync(Stock model);
        Task<Stock?> UpdateAsync (int id, Stock updatedModel);
        
    }
}