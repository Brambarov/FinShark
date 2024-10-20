using api.Extensions;
using api.Models;
using api.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IStocksRepository _stocksRepository;
        private readonly IPortfoliosRepository _portfoliosRepository;

        public PortfoliosController(UserManager<User> userManager,
                                    IStocksRepository stocksRepository,
                                    IPortfoliosRepository portfoliosRepository)
        {
            _userManager = userManager;
            _stocksRepository = stocksRepository;
            _portfoliosRepository = portfoliosRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByUserId()
        {
            var userName = User.GetUserName();

            var user = await _userManager.FindByNameAsync(userName);

            var userPortfolio = await _portfoliosRepository.GetPortfolioByUser(user);

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string symbol)
        {
            var userName = User.GetUserName();

            var user = await _userManager.FindByNameAsync(userName);

            var stock = await _stocksRepository.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                return BadRequest("Stock not found!");
            }

            var userPortfolio = await _portfoliosRepository.GetPortfolioByUser(user);

            if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already in portfolio!");
            }

            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                UserId = user.Id,
            };

            var model = await _portfoliosRepository.CreateAsync(portfolio);

            if (model == null)
            {
                return StatusCode(500, "Could not create portfolio!");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var userName = User.GetUserName();

            var user = await _userManager.FindByNameAsync(userName);

            var portfolio = await _portfoliosRepository.GetPortfolioByUser(user);

            var stocks = portfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (stocks.Count() == 1)
            {
                await _portfoliosRepository.DeleteAsync(user, symbol);
            }
            else
            {
                return BadRequest("Stock is not in portfolio!");
            }

            return Ok();
        }
    }
}
