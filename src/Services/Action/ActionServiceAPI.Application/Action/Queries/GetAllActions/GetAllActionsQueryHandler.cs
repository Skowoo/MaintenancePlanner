using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Action.Queries.GetAllActions
{
    public class GetAllActionsQueryHandler(IActionContext context, IMapper mapper) : IRequestHandler<GetAllActionsQuery, IEnumerable<ActionDto>>
    {
        public async Task<IEnumerable<ActionDto>> Handle(GetAllActionsQuery request, CancellationToken cancellationToken)
        {
            List<ActionEntity> data = [.. await context.Actions
                .Include(x => x.Parts)
                .Include(x => x.CreatedBy)
                .Include(x => x.ConductedBy)
                .ToListAsync(cancellationToken)];

            return data.Select(mapper.Map<ActionDto>).ToList();
        }
    }
}
