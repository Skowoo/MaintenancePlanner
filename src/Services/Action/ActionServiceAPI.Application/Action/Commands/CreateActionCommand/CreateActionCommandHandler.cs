using ActionServiceAPI.Application.IntegrationEvents;
using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.CreateActionCommand
{
    public class CreateActionCommandHandler(IActionContext context, IMediator mediator, IIntegrationEventService integrationEventService) : IRequestHandler<CreateActionCommand, int>
    {
        public async Task<int> Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            var creator = context.Employees.FirstOrDefault(e => e.UserId == request.CreatedBy);

            if (creator is null)  // Refactor - bring back control
            {
                creator = new Employee("string");
                context.Employees.Add(creator);
                await context.SaveChangesAsync(cancellationToken);
            }

            var conductor = context.Employees.FirstOrDefault(e => e.UserId == request.ConductedBy);

            ActionEntity newItem = new(request.Name, request.Description, request.StartDate, request.EndDate, creator, conductor);

            // Refactor - Publishing can be refactorized to avoid changes in future
            await mediator.Publish(new NewActionCreatedDomainEvent(request.Parts), cancellationToken);

            // Refactor - made proper constructor
            var sparePartsUsedEvent = new SparePartsUsedInActionIntegrationEvent()
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                UsedParts = request.Parts.ToList()
            };
            integrationEventService.Publish(sparePartsUsedEvent);


            foreach (var part in request.Parts)
                newItem.AddPart(part);

            context.Actions.Add(newItem);
            await context.SaveChangesAsync(cancellationToken);

            return newItem.Id;
        }
    }
}
