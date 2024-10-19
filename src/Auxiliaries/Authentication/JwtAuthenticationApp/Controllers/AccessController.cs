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
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Login([FromBody] LoginModel user)
        {
            var confirmedUser = loginService.ConfirmUser(user);

            if (confirmedUser is null)
            {
                logger.LogInformation("Login of user {} failed", user.Login);
                return BadRequest("Login failed!");
            }
            else
            {
                logger.LogInformation("Login of user {} suceeded and token was returned", user.Login);
                return Ok(tokenService.GetJwtTokenString((UserModel)confirmedUser));
            }
        }
    }
}
