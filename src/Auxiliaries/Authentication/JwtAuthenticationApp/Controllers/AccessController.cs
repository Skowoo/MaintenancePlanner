using JwtAuthenticationApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JwtAuthenticationApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessController(ILogger<AccessController> logger) : ControllerBase
    {
        private readonly ILogger<AccessController> _logger = logger;

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            return BadRequest();
        }
    }
}
