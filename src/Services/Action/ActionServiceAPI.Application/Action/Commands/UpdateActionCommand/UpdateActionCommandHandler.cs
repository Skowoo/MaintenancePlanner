using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommandHandler(IActionContext context, IMediator mediator) : IRequestHandler<UpdateActionCommand, bool>
    {
        public async Task<bool> Handle(UpdateActionCommand request, CancellationToken cancellationToken)
        {
            var target = context.Actions.SingleOrDefault(x => x.Id == request.Id)
                ?? throw new ActionDomainException("Action not found in database!");

            var creator = context.Employees.SingleOrDefault(e => e.UserId == request.CreatedBy)
                ?? throw new ActionDomainException("Creator not found in database!");

            var conductor = context.Employees.SingleOrDefault(e => e.UserId == request.ConductedBy);

            context.Actions.Entry(target).CurrentValues.SetValues(request);

            target.CreatedBy = creator;
            target.ConductedBy = conductor;
            target.UpdatePartsList(request.Parts);

            await context.SaveChangesAsync(cancellationToken);

            if (request.NewUsedParts.Any())
                await mediator.Publish(new SparePartsTakenDomainEvent(request.NewUsedParts), cancellationToken);

            if (request.ReturnedParts.Any())
                await mediator.Publish(new SparePartsReturnedDomainEvent(request.ReturnedParts), cancellationToken);

            return true;
        }
    }
}
