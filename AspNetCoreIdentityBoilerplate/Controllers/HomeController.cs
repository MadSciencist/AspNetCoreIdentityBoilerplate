using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityBoilerplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public IActionResult Admin()
        {
            return Ok();
        }

        [Authorize(Roles = "user")]
        [HttpGet("user")]
        public IActionResult Users()
        {
            return Ok();
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("Both")]
        public IActionResult Both()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("Anon")]
        public IActionResult Anon()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("Auth")]
        public IActionResult Auth()
        {
            return Ok();
        }
    }
}