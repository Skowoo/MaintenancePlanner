using ActionServiceAPI.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ActionServiceAPI.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void SeedDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ActionContext>();

            context.Database.EnsureDeleted();  // Test Db layout, Refactor to migrations, move connection string, remove passwords from code.
            context.Database.EnsureCreated();

            Employee employee = new("string");
            context.Employees.Add(employee);
            context.SaveChanges();

            AvailablePart availablePart = new(1, 9);
            context.AvailableParts.Add(availablePart);
            context.SaveChanges();

            ActionEntity entity = new("Naprawa", "Wymiana czujnika", DateTime.Now, DateTime.Now, employee, employee);
            UsedPart usedPart = new(1, 1);
            entity.AddPart(usedPart);
            context.Actions.Add(entity);
            context.SaveChanges();
        }
    }
}