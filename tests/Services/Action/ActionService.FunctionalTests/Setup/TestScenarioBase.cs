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

            AvailablePart testPart = new (1, 10);
            context.AvailableParts.Add(testPart);

            context.SaveChanges();

            Employee testEmployee = new("TestEmployee");
            context.Employees.Add(testEmployee);
            context.SaveChanges();

            var testAction = new ActionEntity("Test Name", "Test Description", new DateTime(2024, 09, 13), new DateTime(2024, 09, 12), testEmployee, testEmployee);
            context.Actions.Add(testAction);
            context.SaveChanges();
        }
    }
}