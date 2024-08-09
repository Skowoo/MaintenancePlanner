using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Exceptions;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommandHandler(IActionContext context) : IRequestHandler<UpdateActionCommand, bool>
    {
        public async Task<bool> Handle(UpdateActionCommand request, CancellationToken cancellationToken)
        {
            var target = context.Actions.SingleOrDefault(x => x.Id == request.Id);

            if (target is null)
                return false;

            var creator = context.Employees.SingleOrDefault(e => e.UserId == request.CreatedBy)
                ?? throw new ActionDomainException("Creator not found in database!");

            var conductor = context.Employees.SingleOrDefault(e => e.UserId == request.ConductedBy);

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
