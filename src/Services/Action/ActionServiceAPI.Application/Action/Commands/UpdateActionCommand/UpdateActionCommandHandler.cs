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
            await context.SaveChangesAsync(cancellationToken);

            var takenParts = request.PartsDifference.Where(x => x.Quantity > 0).ToList();
            if (takenParts.Count != 0)
                await mediator.Publish(new SparePartsTakenDomainEvent(takenParts), cancellationToken);

            var returnedParts = request.PartsDifference.Where(x => x.Quantity < 0).ToList();
            if (returnedParts.Count != 0)
            {
                foreach (var part in returnedParts) // Sign have to be changed to positive
                    part.Quantity = -part.Quantity;

                await mediator.Publish(new SparePartsReturnedDomainEvent(returnedParts), cancellationToken);
            }

            return true;
        }
    }
}
