using IdentityServiceAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServiceAPI.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class IdentityController(IIdentityService identityService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await identityService.FindUserByEmail(email);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await identityService.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var role = await identityService.FindRoleByName(roleName);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await identityService.FindUserByEmail(email);
            if (user is null)
                return NotFound(email);            

            var role = await identityService.FindRoleByName(roleName);
            if (role is null)
                return NotFound(roleName);
            
            var result = await identityService.AddUserToRole(user, roleName);
            if (result.Succeeded)
                return Ok();            

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await identityService.FindUserByEmail(email);
            if (user is null)
                return NotFound(email);            

            var role = await identityService.FindRoleByName(roleName);
            if (role is null)
                return NotFound(roleName);
            
            var result = await identityService.RemoveUserFromRole(user, roleName);
            if (result.Succeeded)
                return Ok();            

            return BadRequest(result);
        }
    }
}
