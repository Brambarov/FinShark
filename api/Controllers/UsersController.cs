using api.DTOs.User;
using api.Models;
using api.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokensService _tokensService;
        private readonly SignInManager<User> _signInManager;

        public UsersController(UserManager<User> userManager,
                               ITokensService tokensService,
                               SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokensService = tokensService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO DTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userToCreate = new User
                {
                    UserName = DTO.Username,
                    Email = DTO.Email
                };

                var createdUser = await _userManager.CreateAsync(userToCreate, DTO.Password);

                if (createdUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(userToCreate, "USER");

                    if (role.Succeeded)
                    {
                        return Ok(
                            new GetUserDTO
                            {
                                UserName = userToCreate.UserName,
                                Email = userToCreate.Email,
                                Token = _tokensService.CreateToken(userToCreate)
                            });
                    }
                    else
                    {
                        return StatusCode(500, role.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == DTO.Username.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid username!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, DTO.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid password!");
            }

            return Ok(
                new GetUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokensService.CreateToken(user)
                });
        }
    }
}
