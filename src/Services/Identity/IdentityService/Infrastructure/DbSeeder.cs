namespace IdentityServiceAPI.Infrastructure
{
    public static class DbSeeder
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

            if (context.Database.CanConnect())
                return;

            context.Database.EnsureCreated();  // Refactor to migrations, move connection string, remove passwords from code.
        }
    }
}
