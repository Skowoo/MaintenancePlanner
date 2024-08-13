using ActionServiceAPI.Application.Interfaces.DataRepositories;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.DeleteActionCommand
{
    public class DeleteActionCommandHandler(IActionContext context) : IRequestHandler<DeleteActionCommand, bool>
    {
        public async Task<bool> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
        {
            var target = context.Actions.SingleOrDefault(x => x.Id == request.Id);

            if (target is null)
                return false;

            context.Actions.Remove(target);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
