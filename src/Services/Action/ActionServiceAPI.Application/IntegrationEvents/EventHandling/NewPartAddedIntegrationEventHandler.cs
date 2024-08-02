using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;

namespace ActionServiceAPI.Application.IntegrationEvents.EventHandling
{
    public class NewPartAddedIntegrationEventHandler(IActionContext context)
    {
        public void Handle(NewPartAddedIntegrationEvent evt)
        {
            UsedPart part = new(evt.PartId, evt.Quantity);
            context.UsedParts.Add(part);
            context.SaveChangesAsync(new CancellationToken()); // Refactor
        }
    }
}
