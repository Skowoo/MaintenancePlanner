using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Queries.GetAllActions
{
    public class GetAllActionsQueryHandler(IActionContext context) : IRequestHandler<GetAllActionsQuery, IEnumerable<ActionEntity>>
    {
        public async Task<IEnumerable<ActionEntity>> Handle(GetAllActionsQuery request, CancellationToken cancellationToken) 
            => [.. await context.Actions
                .Include(x => x.Parts)
                .Include(x => x.CreatedBy)
                .Include(x => x.ConductedBy)
                .ToListAsync(cancellationToken)];
    }
}
