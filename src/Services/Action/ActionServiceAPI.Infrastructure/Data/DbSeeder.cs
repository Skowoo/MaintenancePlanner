using Microsoft.Extensions.DependencyInjection;

namespace ActionServiceAPI.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void SeedDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ActionContext>();

            context.Database.EnsureDeleted();  // Test Db layout
            context.Database.EnsureCreated();  // Refactor to migrations, move connection string, remove passwords from code.
        }
    }
}
