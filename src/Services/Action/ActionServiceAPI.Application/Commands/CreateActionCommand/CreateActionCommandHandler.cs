using ActionServiceAPI.Application.Interfaces;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Commands.CreateActionCommand
{
    public class CreateActionCommandHandler(IActionContext context) : IRequestHandler<CreateActionCommand>
    {
        public async Task Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            // Refactor
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

            // Refactor

            ActionEntity newItem = new(request.Name, request.Description, request.StartDate, request.EndDate, creator, conductor);

            context.Actions.Add(newItem);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
