using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommandHandler(IActionContext context) : IRequestHandler<UpdateActionCommand, bool>
    {
        public async Task<bool> Handle(UpdateActionCommand request, CancellationToken cancellationToken)
        {
            var target = await context.Actions.FindAsync(request.Id, cancellationToken);

            if (target is null)
                return false;

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

            target.Name = request.Name;
            target.Description = request.Description;
            target.StartDate = request.StartDate;
            target.EndDate = request.EndDate;
            target.CreatedBy = creator;
            target.ConductedBy = conductor;
            target.UpdatePartsList(request.Parts);

            context.Actions.Update(target);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
