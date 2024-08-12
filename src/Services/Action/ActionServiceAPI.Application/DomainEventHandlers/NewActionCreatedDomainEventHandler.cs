using ActionServiceAPI.Application.IntegrationEvents;
using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using MediatR;

namespace ActionServiceAPI.Application.DomainEventHandlers
{
    public class NewActionCreatedDomainEventHandler(IActionContext context, IIntegrationEventService integrationEventService) : INotificationHandler<NewActionCreatedDomainEvent>
    {
        public async Task Handle(NewActionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            foreach (var requestedPart in notification.Parts)
            {
                var storedPart = context.AvailableParts.FirstOrDefault(p => p.PartId == requestedPart.PartId)
                    ?? throw new ActionDomainException("Part not found");

                if (storedPart.Quantity < requestedPart.Quantity)
                    throw new ActionDomainException("Not enough parts in stock");

                storedPart.Quantity -= requestedPart.Quantity;
            }

            await context.SaveChangesAsync(cancellationToken);

            // Refactor - Publishing can be refactorized to avoid changes in future
            integrationEventService.PublishIntegrationEvent(new SparePartsUsedInActionIntegrationEvent(notification.Parts.ToList()));
        }
    }
}
