using Grpc.Net.Client;
using JwtAuthenticationApp.Models;

namespace JwtAuthenticationApp.Services
{
    public class LoginGrpcService
    {
        readonly string IdentityServiceAddress = "https://IdentityServiceAPI:81"; // Refactor as external variable

        public UserModel? ConfirmUser(LoginModel model)
        {
            // Refactor - HttpClientHandler that disables SSL certificate validation to allow connections. Not for production use.
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

            if (response is not null && response.IsValid)
                return new(response.Id, response.Login, [.. response.Roles]);
            else
                return null;
        }
    }
}
