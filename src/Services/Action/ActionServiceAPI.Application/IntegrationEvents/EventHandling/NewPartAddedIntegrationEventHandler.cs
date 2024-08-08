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
            var part = context.UsedParts.SingleOrDefault(x => x.PartId == evt.PartId);
            if (part is null)
            {
                part = new UsedPart(evt.PartId, evt.Quantity);
                context.UsedParts.Add(part);
            }
            else
            {
                part.Quantity += evt.Quantity;
            }
            
            context.SaveChangesAsync(new CancellationToken()); // Refactor
            return Task.CompletedTask;
        }
    }
}
