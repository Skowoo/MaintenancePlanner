using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Services
{
    public class IdentityService(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager
        ) : IIdentityService
    {
        public async Task<IdentityResult> RegisterNewUser (RegisterModel user, string password)
        {
            ApplicationUser newUser = new()
            {
                UserName = user.Login,
                Email = user.Email
            };

            return await userManager.CreateAsync(newUser, password);
        }

        public async Task<IdentityResult> LoginUser(string login, string password)
        {
            var user = await FindUserByName(login);

            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var loginSuccessed = await userManager.CheckPasswordAsync(user, password);

            if (loginSuccessed)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError { Description = "Invalid password" });
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers() => await userManager.Users.ToArrayAsync();

        public async Task<ApplicationUser?> FindUserByName(string username) => await userManager.FindByNameAsync(username);

        public async Task<IEnumerable<IdentityRole>> GetAllRoles() => await roleManager.Roles.ToArrayAsync();

        public async Task<IdentityRole?> FindRoleByName(string roleName) => await roleManager.FindByNameAsync(roleName);

        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName) => await userManager.AddToRoleAsync(user, roleName);

        public async Task<IdentityResult> RemoveUserFromRole(ApplicationUser user, string roleName) => await userManager.RemoveFromRoleAsync(user, roleName);
    }
}
