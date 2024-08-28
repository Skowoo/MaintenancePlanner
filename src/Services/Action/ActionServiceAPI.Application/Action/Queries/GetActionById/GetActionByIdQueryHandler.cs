using ActionServiceAPI.Application.DataTransferObjects.Models;
using ActionServiceAPI.Application.Interfaces.DataRepositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Action.Queries.GetActionById
{
    public class GetActionByIdQueryHandler(IActionContext context, IMapper mapper) : IRequestHandler<GetActionByIdQuery, ActionDto?>
    {
        public async Task<ActionDto?> Handle(GetActionByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await context.Actions
                .Include(x => x.Parts)
                .Include(x => x.CreatedBy)
                .Include(x => x.ConductedBy)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return data is not null ? mapper.Map<ActionDto>(data) : null;
        }

    }
}
