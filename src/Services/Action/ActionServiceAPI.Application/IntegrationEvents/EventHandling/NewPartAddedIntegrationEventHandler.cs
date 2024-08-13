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
            var part = context.AvailableParts.SingleOrDefault(x => x.PartId == evt.PartId);
            if (part is null)
            {
                part = new AvailablePart()
                {
                    PartId = evt.PartId,
                    Quantity = evt.Quantity
                };
                context.AvailableParts.Add(part);
            }
            else
            {
                part.Quantity += evt.Quantity;
            }

            context.SaveChangesAsync(CancellationToken.None);
            return Task.CompletedTask;
        }
    }
}
