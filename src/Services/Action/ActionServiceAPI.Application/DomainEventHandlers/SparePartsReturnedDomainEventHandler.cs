using ActionServiceAPI.Application.IntegrationEvents;
using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using MediatR;

namespace ActionServiceAPI.Application.DomainEventHandlers
{
    public class SparePartsReturnedDomainEventHandler(IActionContext context, IIntegrationEventService integrationEventService) : INotificationHandler<SparePartsReturnedDomainEvent>
    {
        public async Task Handle(SparePartsReturnedDomainEvent notification, CancellationToken cancellationToken)
        {
            foreach (var requestedPart in notification.Parts)
            {
                var storedPart = context.AvailableParts.FirstOrDefault(p => p.PartId == requestedPart.PartId)
                    ?? throw new ActionDomainException("Part not found");

                storedPart.Quantity += requestedPart.Quantity;
            }

            await context.SaveChangesAsync(cancellationToken);

            // Refactor - Publishing can be refactorized to avoid changes in future
            integrationEventService.PublishIntegrationEvent(new SparePartsReturnedFromActionIntegrationEvent(notification.Parts.ToList()));
        }
    }
}