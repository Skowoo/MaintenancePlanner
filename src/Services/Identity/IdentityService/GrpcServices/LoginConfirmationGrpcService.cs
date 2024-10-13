using Grpc.Core;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;

namespace IdentityServiceAPI.GrpcServices
{
    public class LoginConfirmationGrpcService(IIdentityService identityService) : UserConfirmator.UserConfirmatorBase
    {
        public override async Task<LoginResponse> Work(LoginRequest request, ServerCallContext context)
        {
            LoginModel loginModel = new(request.Login, request.Password);
            var result = await identityService.LoginUser(loginModel);

            if (result.Succeeded) 
            {
                var user = await identityService.FindUserByUserName(request.Login);
                var roles = await identityService.GetUserRoles(user!);

                LoginResponse response = new() { IsValid = true };

                foreach (var role in roles) 
                    response.Roles.Add(role);
                
                return response;
            }
            else
            {
                return new LoginResponse
                {
                    IsValid = false
                };
            }
        }
    }
}
