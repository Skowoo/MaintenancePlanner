using IdentityServiceAPI.IntegrationEvents;
using IdentityServiceAPI.IntegrationEvents.Events;
using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Services
{
    public class IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IIntegrationEventService integrationEventService,
        ILogger<IdentityService> logger
        ) : IIdentityService
    {
        readonly UserManager<ApplicationUser> _userManager = userManager;
        readonly RoleManager<IdentityRole> _roleManager = roleManager;
        readonly IIntegrationEventService _integrationEventService = integrationEventService;
        readonly ILogger<IdentityService> _logger = logger;

        public async Task<(IdentityResult Result, string? NewUserId)> RegisterNewUser(RegisterModel registerModel)
        {
            ApplicationUser newUser = new()
            {
                UserName = registerModel.Login,
                Email = registerModel.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);

            if (result.Succeeded)
            {
                _integrationEventService.PublishIntegrationEvent(new NewUserCreatedIntegrationEvent(newUser.Id));
                _logger.LogTrace("New user created with Id: {}", newUser.Id);
            }
            else
                _logger.LogTrace("New user registration attempt failed.");

            return (result, newUser.Id);
        }

        public async Task<IdentityResult> LoginUser(LoginModel loginModel)
        {
            var user = await FindUserByUserName(loginModel.Login);

            if (user is null)
            {
                _logger.LogTrace("Invalid login attempt. User not found in database");
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var loginSuccessed = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            if (loginSuccessed)
            {
                _logger.LogTrace("User with Id: {} logged succesfully.", user.Id);
                return IdentityResult.Success;
            }
            else
            {
                _logger.LogTrace("Login of user with Id: {} failed due to invalid password", user.Id);
                return IdentityResult.Failed(new IdentityError { Description = "Invalid password" });
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers() => await _userManager.Users.ToArrayAsync();

        public async Task<ApplicationUser?> FindUserByUserName(string username) => await _userManager.FindByNameAsync(username);

        public async Task<IEnumerable<IdentityRole>> GetAllRoles() => await _roleManager.Roles.ToArrayAsync();

        public async Task<IdentityRole?> FindRoleByName(string roleName) => await _roleManager.FindByNameAsync(roleName);

        public async Task<IdentityResult> AddUserToRole(string userName, string roleName)
        {
            var user = await FindUserByUserName(userName);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNameNotFound", Description = "User not found" });

            var role = await FindRoleByName(roleName);
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Code = "InvalidRoleName", Description = $"Role not found" });

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRole(string userName, string roleName)
        {
            var user = await FindUserByUserName(userName);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNameNotFound", Description = "User not found" });

            var role = await FindRoleByName(roleName);
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Code = "InvalidRoleName", Description = $"Role not found" });

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUser user) => await _userManager.GetRolesAsync(user);
    }
}
