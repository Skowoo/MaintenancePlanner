using ActionServiceAPI.Application.DataTransferObjects.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Queries.GetAllActions
{
    public class GetAllActionsQuery : IRequest<IEnumerable<ActionDto>>
    {
    }
}
