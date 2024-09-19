using IdentityServiceAPI.Infrastructure;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.FunctionalTests.Setup
{
    public class TestScenarioBase
    {
        public const string IdentityServiceUri = "https://localhost:7001/api/v1/Identity/";
        public const string AdminLogin = "Admin";
        public const string AdminEmail = "admin@adm.eu";
        public const string AdminPassword = "Adm1n3k!!!";
        public const string AdminRoleName = "Admin";


        protected static HttpClient GetClient()
        {
            var factory = new IdentityServiceFactory();
            var scope = factory.Services.CreateScope();
            SeedDatabase(scope);
            return factory.CreateClient();
        }

        static void SeedDatabase(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            context.Database.EnsureCreated();

            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

            var adminRegisterModel = new RegisterModel()
            {
                Login = AdminLogin,
                Email = AdminEmail,
                Password = AdminPassword
            };
            identityService.RegisterNewUser(adminRegisterModel, AdminPassword).Wait();

            var role = new IdentityRole(AdminRoleName);
            context.Roles.Add(role);
            context.SaveChanges();

            //var admin = context.Users.Single(u => u.UserName == AdminLogin);
            //identityService.AddUserToRole(admin, AdminRoleName).Wait();
            //context.SaveChanges();
        }
    }
}
