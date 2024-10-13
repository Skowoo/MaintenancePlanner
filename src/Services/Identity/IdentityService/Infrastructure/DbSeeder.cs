using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceAPI.Infrastructure
{
    public static class DbSeeder
    {
        private const string AdminLogin = "Admin";
        private const string AdminEmail = "admin@adm.eu";
        private const string AdminPassword = "Adm1n3k!!!";

        private const string NoRoleUserLogin = "NoRoleUser";
        private const string NoRoleUserEmail = "NoRoleUser@nru.eu";
        private const string NoRoleUserPassword = "N0R0leUs3r!!!";

        private const string AdminRoleName = "Admin";

        public static void SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

            context.Database.EnsureDeleted();  // Test Db layout
            context.Database.EnsureCreated();  // Refactor to migrations, move connection string, remove passwords from code.

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
