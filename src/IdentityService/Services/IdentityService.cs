using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Services
{
    public class IdentityService(
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager
        ) : IIdentityService
    {
        public async Task<IdentityUser?> FindUserByEmail(string email) => await userManager.FindByEmailAsync(email);

        public async Task<IEnumerable<IdentityRole>> GetAllRoles() => await roleManager.Roles.ToArrayAsync();

        public async Task<IdentityRole?> FindRoleByName(string roleName) => await roleManager.FindByNameAsync(roleName);

        public async Task<IdentityResult> AddUserToRole(IdentityUser user, string roleName) => await userManager.AddToRoleAsync(user, roleName);

        public async Task<IdentityResult> RemoveUserFromRole(IdentityUser user, string roleName) => await userManager.RemoveFromRoleAsync(user, roleName);
    }
}
