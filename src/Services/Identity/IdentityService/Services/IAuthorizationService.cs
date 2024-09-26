
namespace IdentityServiceAPI.Services
{
    public interface IAuthorizationService
    {
        Task<string> GetJwtTokenAsync(string userName);
    }
}