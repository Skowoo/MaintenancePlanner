using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Queries.GetAllActions
{
    public class GetAllActionsQuery : IRequest<IEnumerable<ActionEntity>>
    {
    }
}
