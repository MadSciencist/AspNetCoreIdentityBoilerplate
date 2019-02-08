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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenBuilder _tokenBuilder;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,
            ITokenBuilder tokenBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenBuilder = tokenBuilder;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login, string redirect)
        {
            var user = await _userManager.FindByNameAsync(login.Login);

            if (user == null) return NotFound();

            await _signInManager.SignOutAsync(); // terminate existing session

            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);

            if (!signInResult.Succeeded) return Unauthorized();

            var (token, expring) = _tokenBuilder.BuildToken(user);

            return Ok(new {access = new {token, expires = expring}, redirect});
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register, string redirect)
        {
            if (await _userManager.FindByNameAsync(register.Login) != null) return BadRequest("User already exists");

            var user = new AppUser
            {
                Email = register.Email,
                UserName = register.Login
            };

            var result =  await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var signInResult = await _signInManager.PasswordSignInAsync(user, register.Password, false, false);
            if (!signInResult.Succeeded) return Unauthorized();

            var (token, expring) = _tokenBuilder.BuildToken(user);

            return CreatedAtAction(nameof(Register), "", new { access = new { token, expires = expring }, redirect });
        }
    }
}
