using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Exceptions;
using MediatR;

namespace ActionServiceAPI.Application.Commands.DeleteActionCommand
{
    public class DeleteActionCommandHandler(IActionContext context) : IRequestHandler<DeleteActionCommand, bool>
    {
        public async Task<bool> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
        {
            var target = await context.Actions.FindAsync(request.Id, cancellationToken);

            if (target is null)
                return false;

            context.Actions.Remove(target);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
