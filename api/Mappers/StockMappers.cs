using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static GetStockDTO ToGetDTO(this Stock model)
        {
            return new GetStockDTO
            {
                Id = model.Id,
                Symbol = model.Symbol,
                CompanyName = model.CompanyName,
                Purchase = model.Purchase,
                LastDiv = model.LastDiv,
                Industry = model.Industry,
                MarketCap = model.MarketCap,
                Comments = model.Comments.Select(c => c.ToGetDTO()).ToList()
            };
        }

        public static Stock FromPostDTO(this PostStockDTO DTO)
        {
            return new Stock
            {
                Symbol = DTO.Symbol,
                CompanyName = DTO.CompanyName,
                Purchase = DTO.Purchase,
                LastDiv = DTO.LastDiv,
                Industry = DTO.Industry,
                MarketCap = DTO.MarketCap
            };
        }

        public static Stock ToModel(this PutStockDTO DTO)
        {
            return new Stock
            {
                Symbol = DTO.Symbol,
                CompanyName = DTO.CompanyName,
                Purchase = DTO.Purchase,
                LastDiv = DTO.LastDiv,
                Industry = DTO.Industry,
                MarketCap = DTO.MarketCap
            };
        }
    }
}