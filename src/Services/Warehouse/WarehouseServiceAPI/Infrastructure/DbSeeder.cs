using WarehouseServiceAPI.Models;

namespace WarehouseServiceAPI.Infrastructure
{
    public static class DbSeeder
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarehouseContext>();

            context.Database.EnsureDeleted();  // Test Db layout, Refactor to migrations, move connection string, remove passwords from code.
            context.Database.EnsureCreated();

            Part part = new("Sensor", "NPN, M8 3pin", "FESTO", "LRM-NUA 32", 9, 5);
            context.Parts.Add(part);
            context.SaveChanges();
        }
    }
}
