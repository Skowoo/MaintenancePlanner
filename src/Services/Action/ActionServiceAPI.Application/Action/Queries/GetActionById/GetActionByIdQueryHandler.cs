using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Action.Queries.GetActionById
{
    public class GetActionByIdQueryHandler(IActionContext context) : IRequestHandler<GetActionByIdQuery, ActionEntity?>
    {
        public async Task<ActionEntity?> Handle(GetActionByIdQuery request, CancellationToken cancellationToken) =>
            await context.Actions
                .Include(x => x.Parts)
                .Include(x => x.CreatedBy)
                .Include(x => x.ConductedBy)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}
