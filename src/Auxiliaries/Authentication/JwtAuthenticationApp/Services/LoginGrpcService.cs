using Grpc.Core;
using Grpc.Net.Client;
using JwtAuthenticationApp.Models;

namespace JwtAuthenticationApp.Services
{
    public class LoginGrpcService
    {
        string IdentityServiceAddress = "https://IdentityServiceAPI:81";

        public (bool, string[]) ConfirmUser(LoginModel model)
        {
            // Bypass of lack of SSL certificate - refactor
            var httpClientHandler = new HttpClientHandler
            {                
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress(IdentityServiceAddress, new GrpcChannelOptions
            {
               HttpClient = httpClient
            });

            var client = new UserConfirmator.UserConfirmatorClient(channel);
            var response = client.Work(new LoginRequest { Login = model.Login, Password = model.Password });
            return (response.IsValid, [.. response.Roles]);
        }
    }
}
