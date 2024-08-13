using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServiceAPI.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class IdentityController(IIdentityService identityService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            var result = await identityService.RegisterNewUser(user, user.Password);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string login, string password)
        {
            var result = await identityService.LoginUser(login, password);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() => Ok(await identityService.GetAllUsers());

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await identityService.FindUserByName(email);
            return user is null ? NotFound(email) : Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var role = await identityService.FindRoleByName(roleName);
            return role is null ? NotFound(roleName) : Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles() => Ok(await identityService.GetAllRoles());

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await identityService.FindUserByName(email);
            if (user is null)
                return NotFound(email);

            var role = await identityService.FindRoleByName(roleName);
            if (role is null)
                return NotFound(roleName);

            var result = await identityService.AddUserToRole(user, roleName);
            return result.Succeeded ? Ok() : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await identityService.FindUserByName(email);
            if (user is null)
                return NotFound(email);

            var role = await identityService.FindRoleByName(roleName);
            if (role is null)
                return NotFound(roleName);

            var result = await identityService.RemoveUserFromRole(user, roleName);
            return result.Succeeded ? Ok() : BadRequest(result);
        }
    }
}
