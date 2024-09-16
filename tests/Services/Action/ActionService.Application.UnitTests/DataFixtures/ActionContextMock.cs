using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using ActionServiceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Application.UnitTests.DataFixtures
{
    internal static class ActionContextMock
    {
        internal const string CreatedByEmployeeId = "d30f82c0-7232-4549-83ee-08c9111aff8e";
        internal const string ConductedByEmployeeId = "7cd45c10-226f-4019-917e-ddcf4085c27f";
        internal const int ExistingPartId = 1;

        public static IActionContext GetContextMock()
        {
            var options = new DbContextOptionsBuilder<ActionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new ActionContext(options);

            context.Database.EnsureCreated();

            Employee creator = new(CreatedByEmployeeId);
            Employee conductor = new(ConductedByEmployeeId);
            context.Employees.Add(creator);
            context.Employees.Add(conductor);

            AvailablePart part = new(ExistingPartId, 10);
            context.AvailableParts.Add(part);

            context.SaveChanges();

            ActionEntity action = new("TestAction", "TestDescription", DateTime.UtcNow, DateTime.UtcNow, creator, conductor);
            UsedPart usedPart = new(ExistingPartId, 4);
            action.AddPart(usedPart);
            context.Actions.Add(action);

            context.SaveChanges();

            return context;
        }
    }
}
