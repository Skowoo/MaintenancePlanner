﻿using AutoMapper;
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
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterModel user)
        {
            var (Result, NewUserId) = await identityService.RegisterNewUser(user, user.Password);
            return Result.Succeeded ? Ok(NewUserId) : BadRequest(Result.Errors);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            var result = await identityService.LoginUser(user.Login, user.Password);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUserExternalModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersList = await identityService.GetAllUsers();
            var mappedToExternalModelUsersList = usersList.Select(mapper.Map<ApplicationUserExternalModel>);
            return Ok(mappedToExternalModelUsersList);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApplicationUserExternalModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var user = await identityService.FindUserByUserName(userName);
            return user is not null ? Ok(mapper.Map<ApplicationUserExternalModel>(user)) : NotFound(userName);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IdentityRole), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var role = await identityService.FindRoleByName(roleName);
            return role is not null ? Ok(role) : NotFound(roleName);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdentityRole>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRoles() => Ok(await identityService.GetAllRoles());

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddUserToRole([FromBody] RoleAssignChangeModel input)
        {
            var user = await identityService.FindUserByUserName(input.Login);
            if (user is null)
                return NotFound($"{nameof(input.Login)} : {input.Login} not found!");

            var role = await identityService.FindRoleByName(input.RoleName);
            if (role is null)
                return NotFound($"{nameof(input.RoleName)} : {input.RoleName} not found!");

            var result = await identityService.AddUserToRole(user, input.RoleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] RoleAssignChangeModel input)
        {
            var user = await identityService.FindUserByUserName(input.Login);
            if (user is null)
                return NotFound($"{nameof(input.Login)} : {input.Login} not found!");

            var role = await identityService.FindRoleByName(input.RoleName);
            if (role is null)
                return NotFound($"{nameof(input.RoleName)} : {input.RoleName} not found!");

            var result = await identityService.RemoveUserFromRole(user, input.RoleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
    }
}
