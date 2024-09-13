using ActionServiceAPI.Domain.Models;
using ActionServiceAPI.Infrastructure.Data;
using ActionServiceAPI.Web;

namespace ActionService.FunctionalTests.Setup
{
    public class TestScenarioBase
    {
        protected static HttpClient GetClient()
        {
            var factory = new ActionServiceFactory<Program>();
            var scope = factory.Services.CreateScope();
            SeedDatabase(scope);
            return factory.CreateClient();
        }

        static void SeedDatabase(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<ActionContext>();
            context.Database.EnsureCreated();

            context.AvailableParts.AddRange(
                new AvailablePart(1, 10)
            );

            context.SaveChanges();

            context.Employees.AddRange(
                new Employee("TestEmployee")
            );

            context.SaveChanges();
        }
    }
}