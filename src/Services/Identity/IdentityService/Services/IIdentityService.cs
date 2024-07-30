using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceAPI.Services
{
    public interface IIdentityService
    {
        Task<IdentityResult> RegisterNewUser(RegisterModel user, string password);

        Task<IdentityResult> LoginUser(string login, string password);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();

        Task<ApplicationUser?> FindUserByName(string username);

        Task<IEnumerable<IdentityRole>> GetAllRoles();

        Task<IdentityRole?> FindRoleByName(string roleName);

        Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName);

        Task<IdentityResult> RemoveUserFromRole(ApplicationUser user, string roleName);        
    }
}
