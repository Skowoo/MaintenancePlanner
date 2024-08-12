using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using EventBus.Abstractions;

namespace ActionServiceAPI.Application.IntegrationEvents.EventHandling
{
    public class NewUserCreatedIntegrationEventHandler(IActionContext context) : IIntegrationEventHandler<NewUserCreatedIntegrationEvent>
    {
        public async Task Handle(NewUserCreatedIntegrationEvent evt)
        {
            var newEmployee = new Employee(evt.UserId);
            context.Employees.Add(newEmployee);
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
