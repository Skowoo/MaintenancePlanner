using ActionServiceAPI.Domain.Models;
using ActionServiceAPI.Infrastructure.Data;
using ActionServiceAPI.Web;

namespace ActionService.FunctionalTests.Setup
{
    public class TestScenarioBase
    {
        public const string ActionServiceUri = "https://localhost:7120/api/v1/Action/";
        public const string FirstEmployeeId = "2a77d29c-98f6-4894-ac08-33c69a7442a3";
        public const string SecondEmployeeId = "962b04d3-d15b-4c64-835e-bae5b821f502";
        public const int AvailableSparePartId = 1;
        public const int AvailableSparePartQuantity = 10;

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

            AvailablePart testPart = new(AvailableSparePartId, AvailableSparePartQuantity);
            context.AvailableParts.Add(testPart);
            context.SaveChanges();

            Employee firstEmployee = new(FirstEmployeeId);
            context.Employees.Add(firstEmployee);
            Employee secondEmployee = new(SecondEmployeeId);
            context.Employees.Add(secondEmployee);
            context.SaveChanges();

            var testAction = new ActionEntity("Test Name", "Test Description", new DateTime(2024, 09, 13), new DateTime(2024, 09, 14), firstEmployee, secondEmployee);
            UsedPart usedPart = new(1, 5);
            testAction.AddPart(usedPart);
            context.Actions.Add(testAction);
            context.SaveChanges();
        }
    }
}