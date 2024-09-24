using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceAPI.Services
{
    public interface IIdentityService
    {
        Task<(IdentityResult Result, string? NewUserId)> RegisterNewUser(RegisterModel registerModel);

        Task<IdentityResult> LoginUser(LoginModel loginModel);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();

        Task<ApplicationUser?> FindUserByUserName(string username);

        Task<IEnumerable<IdentityRole>> GetAllRoles();

        Task<IdentityRole?> FindRoleByName(string roleName);

        Task<IdentityResult> AddUserToRole(string user, string roleName);

        Task<IdentityResult> RemoveUserFromRole(string user, string roleName);
    }
}
