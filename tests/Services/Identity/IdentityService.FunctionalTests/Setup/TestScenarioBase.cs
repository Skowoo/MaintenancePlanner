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

        public const string NoRoleUserLogin = "NoRoleUser";
        public const string NoRoleUserEmail = "NoRoleUser@nru.eu";
        public const string NoRoleUserPassword = "N0R0leUs3r!!!";

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
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

            var adminRegisterModel = new RegisterModel()
            {
                Login = AdminLogin,
                Email = AdminEmail,
                Password = AdminPassword
            };
            identityService.RegisterNewUser(adminRegisterModel).Wait();

            var noRoleUserRegisterModel = new RegisterModel()
            {
                Login = NoRoleUserLogin,
                Email = NoRoleUserEmail,
                Password = NoRoleUserPassword
            };
            identityService.RegisterNewUser(noRoleUserRegisterModel).Wait();

            roleManager.CreateAsync(new IdentityRole(AdminRoleName)).Wait();
            context.SaveChanges();

            var admin = context.Users.Single(u => u.UserName == AdminLogin);
            identityService.AddUserToRole(AdminLogin, AdminRoleName).Wait();
            context.SaveChanges();
        }
    }
}
