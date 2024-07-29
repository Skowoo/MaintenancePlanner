using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceAPI.Services
{
    public interface IIdentityService
    {
        Task<IdentityUser?> FindUserByEmail(string email);

        Task<IEnumerable<IdentityRole>> GetAllRoles();

        Task<IdentityRole?> FindRoleByName(string roleName);

        Task<IdentityResult> AddUserToRole(IdentityUser user, string roleName);

        Task<IdentityResult> RemoveUserFromRole(IdentityUser user, string roleName);        
    }
}
