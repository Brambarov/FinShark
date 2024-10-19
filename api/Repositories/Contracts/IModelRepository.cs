using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;
using api.Models.Contracts;

namespace api.Repositories.Contracts
{
    public interface IModelRepository
    {
        Task<List<IModel>> GetAllAsync();
        Task<IModel?> GetByIdAsync(int id);
        Task<IModel?> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}