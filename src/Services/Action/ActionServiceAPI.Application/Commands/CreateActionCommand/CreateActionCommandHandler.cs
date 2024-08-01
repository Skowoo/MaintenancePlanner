using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Commands.CreateActionCommand
{
    public class CreateActionCommandHandler(IActionContext context, IMediator mediator) : IRequestHandler<CreateActionCommand>
    {
        public async Task Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            // Refactor - Validation and cretion should be moved
            var creator = context.Employees.FirstOrDefault(e => e.UserId == request.CreatedBy);
            if (creator is null)
            {
                creator = new Employee(request.CreatedBy);
                context.Employees.Add(creator);
                await context.SaveChangesAsync(cancellationToken);
            }                

            var conductor = context.Employees.FirstOrDefault(e => e.UserId == request.ConductedBy);
            if (conductor is null && request.ConductedBy is not null)
            {
                conductor = new Employee(request.ConductedBy);
                context.Employees.Add(conductor);
                await context.SaveChangesAsync(cancellationToken);
            }

            ActionEntity newItem = new(request.Name, request.Description, request.StartDate, request.EndDate, creator, conductor);

            context.Actions.Add(newItem);
            await context.SaveChangesAsync(cancellationToken);

            // Refactor - Publishing can be refactorized to avoid changes in future
            await mediator.Publish(new NewActionCreatedDomainEvent(request.Parts), cancellationToken);
        }
    }
}
