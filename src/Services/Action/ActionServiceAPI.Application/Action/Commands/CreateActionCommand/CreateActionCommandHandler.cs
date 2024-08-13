using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.CreateActionCommand
{
    public class CreateActionCommandHandler(IActionContext context, IMediator mediator) : IRequestHandler<CreateActionCommand, int>
    {
        public async Task<int> Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            var creator = context.Employees.SingleOrDefault(e => e.UserId == request.CreatedBy)
                ?? throw new ActionDomainException("Creator not found");

            var conductor = context.Employees.SingleOrDefault(e => e.UserId == request.ConductedBy);

            ActionEntity newItem = new(request.Name, request.Description, request.StartDate, request.EndDate, creator, conductor);

            foreach (var part in request.Parts)
                newItem.AddPart(part);

            context.Actions.Add(newItem);
            await context.SaveChangesAsync(cancellationToken);

            // Refactor - Publishing can be refactorized to avoid changes in future
            await mediator.Publish(new NewActionCreatedDomainEvent(request.Parts), cancellationToken);

            return newItem.Id;
        }
    }
}
