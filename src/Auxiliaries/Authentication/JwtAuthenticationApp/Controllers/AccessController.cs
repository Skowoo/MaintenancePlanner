using JwtAuthenticationApp.Models;
using JwtAuthenticationApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JwtAuthenticationApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccessController(ILogger<AccessController> logger, LoginGrpcService loginService, JwtTokenService tokenService) : ControllerBase
    {
        private readonly ILogger<AccessController> _logger = logger;

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Login([FromBody] LoginModel user)
        {
            var (result, id, login, roles) = loginService.ConfirmUser(user);

            if (!result)
                return BadRequest("Login failed!");

            return Ok(tokenService.GetJwtTokenString(id, login, roles));
        }
    }
}
