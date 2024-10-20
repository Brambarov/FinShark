using api.Models;

namespace api.Services.Contracts
{
    public interface ITokensService
    {
        string CreateToken(User user);
    }
}
