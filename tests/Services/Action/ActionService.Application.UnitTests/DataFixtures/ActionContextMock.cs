using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using ActionServiceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Application.UnitTests.DataFixtures
{
    internal static class ActionContextMock
    {
        internal const string ExistingEmployeeName = "TestEmployee";
        internal const int ExistingPartId = 1;

        public static IActionContext GetContextMock()
        {
            var options = new DbContextOptionsBuilder<ActionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new ActionContext(options);

            context.Database.EnsureCreated();

            Employee employee = new(ExistingEmployeeName);
            context.Employees.Add(employee);

            AvailablePart part = new() 
            {
                PartId = ExistingPartId, 
                Quantity = 10 
            };
            context.AvailableParts.Add(part);

            context.SaveChanges();

            ActionEntity action = new("TestAction", "TestDescription", DateTime.Now, DateTime.Now, employee, null);
            context.Actions.Add(action);

            context.SaveChanges();

            return context;
        }
    }
}
