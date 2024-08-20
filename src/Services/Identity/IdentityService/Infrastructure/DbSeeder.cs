namespace IdentityServiceAPI.Infrastructure
{
    public static class DbSeeder
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

            context.Database.EnsureDeleted();  // Test Db layout
            context.Database.EnsureCreated();  // Refactor to migrations, move connection string, remove passwords from code.
        }
    }
}
