using AutoMapper;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IdentityServiceAPI.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class IdentityController(IIdentityService identityService, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            var (Result, NewUserId) = await identityService.RegisterNewUser(user, user.Password);

            if (Result.Succeeded)
                return Ok(NewUserId);

            return BadRequest(Result.Errors);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(string login, string password)
        {
            var result = await identityService.LoginUser(login, password);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUserExternalModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersList = await identityService.GetAllUsers();
            return Ok(usersList.Select(mapper.Map<ApplicationUserExternalModel>));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApplicationUserExternalModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var user = await identityService.FindUserByUserName(userName);
            return user is null ? NotFound(userName) : Ok(mapper.Map<ApplicationUserExternalModel>(user));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApplicationUserExternalModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var role = await identityService.FindRoleByName(roleName);
            return role is null ? NotFound(roleName) : Ok(role);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdentityRole>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRoles() => Ok(await identityService.GetAllRoles());

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await identityService.FindUserByUserName(email);
            if (user is null)
                return NotFound(email);

            var role = await identityService.FindRoleByName(roleName);
            if (role is null)
                return NotFound(roleName);

            var result = await identityService.AddUserToRole(user, roleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await identityService.FindUserByUserName(email);
            if (user is null)
                return NotFound(email);

            var role = await identityService.FindRoleByName(roleName);
            if (role is null)
                return NotFound(roleName);

            var result = await identityService.RemoveUserFromRole(user, roleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
    }
}
