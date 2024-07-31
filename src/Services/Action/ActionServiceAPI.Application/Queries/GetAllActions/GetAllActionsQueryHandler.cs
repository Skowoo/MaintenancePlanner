using ActionServiceAPI.Application.Interfaces;
using ActionServiceAPI.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Queries.GetAllActions
{
    public class GetAllActionsQueryHandler(IActionContext context) : IRequestHandler<GetAllActionsQuery, IEnumerable<ActionEntity>>
    {
        public async Task<IEnumerable<ActionEntity>> Handle(GetAllActionsQuery request, CancellationToken cancellationToken) 
            => [.. await context.Actions.ToListAsync(cancellationToken)];
    }
}
