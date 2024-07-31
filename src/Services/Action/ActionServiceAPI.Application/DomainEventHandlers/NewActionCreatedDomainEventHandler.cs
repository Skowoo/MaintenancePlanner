using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.DomainEventHandlers
{
    public class NewActionCreatedDomainEventHandler(IActionContext context) : INotificationHandler<NewActionCreatedDomainEvent>
    {
        public async Task Handle(NewActionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var creator = context.Employees.FirstOrDefault(e => e.UserId == notification.CreatorId);
            if (creator is null)
            {
                creator = new Employee(notification.CreatorId);
                context.Employees.Add(creator);
                await context.SaveChangesAsync(cancellationToken);
            }

            var conductor = context.Employees.FirstOrDefault(e => e.UserId == notification.ConductorId);
            if (conductor is null && notification.ConductorId is not null)
            {
                conductor = new Employee(notification.ConductorId);
                context.Employees.Add(conductor);
                await context.SaveChangesAsync(cancellationToken);
            }

            foreach (var requestedPart in notification.Parts)
            {
                var storedPart = context.UsedParts.FirstOrDefault(p => p.PartId == requestedPart.PartId) 
                    ?? throw new ActionDomainException("Part not found");

                if (storedPart.Quantity < requestedPart.Quantity)
                    throw new ActionDomainException("Not enough parts in stock");                
            }

            // Refactor - TBD : Integration Event to remove parts from stock
        }
    }
}
