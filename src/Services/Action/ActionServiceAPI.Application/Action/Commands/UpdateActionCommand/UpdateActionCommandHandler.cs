using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Events;
using ActionServiceAPI.Domain.Exceptions;
using ActionServiceAPI.Domain.Models;
using AutoMapper;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommandHandler(IActionContext context, IMediator mediator, IMapper mapper) : IRequestHandler<UpdateActionCommand, bool>
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
            target.UpdatePartsList(request.Parts.Select(mapper.Map<UsedPart>));

            await context.SaveChangesAsync(cancellationToken);

            if (request.GetNewUsedPartsList().Count != 0)
                await mediator.Publish(new SparePartsTakenDomainEvent(request.GetNewUsedPartsList().Select(mapper.Map<UsedPart>)), cancellationToken);

            if (request.GetReturnedPartsList().Count != 0)
                await mediator.Publish(new SparePartsReturnedDomainEvent(request.GetReturnedPartsList().Select(mapper.Map<UsedPart>)), cancellationToken);

            return true;
        }
    }
}
