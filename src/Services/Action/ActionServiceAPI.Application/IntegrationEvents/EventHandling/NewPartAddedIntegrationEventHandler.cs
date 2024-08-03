using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using EventBus.Abstractions;

namespace ActionServiceAPI.Application.IntegrationEvents.EventHandling
{
    public class NewPartAddedIntegrationEventHandler(IActionContext context) : IIntegrationEventHandler<NewPartAddedIntegrationEvent>
    {
        public Task Handle(NewPartAddedIntegrationEvent evt)
        {
            UsedPart part = new(evt.PartId, evt.Quantity);
            context.UsedParts.Add(part);
            context.SaveChangesAsync(new CancellationToken()); // Refactor
            return Task.CompletedTask;
        }
    }
}
