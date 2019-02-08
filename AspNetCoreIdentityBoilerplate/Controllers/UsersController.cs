using AspNetCoreIdentityBoilerplate.Infrastructure;
using AspNetCoreIdentityBoilerplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreIdentityBoilerplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenBuilder _tokenBuilder;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenBuilder tokenBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenBuilder = tokenBuilder;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Login);

            if (user == null)
                return NotFound();

            await _signInManager.SignOutAsync(); // terminate existing session

            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);

            if (!signInResult.Succeeded) return Unauthorized();

            var (token, expring) = _tokenBuilder.BuildToken(user);

            return Ok(new { access = new {token, expires = expring} });
        }
    }
}
