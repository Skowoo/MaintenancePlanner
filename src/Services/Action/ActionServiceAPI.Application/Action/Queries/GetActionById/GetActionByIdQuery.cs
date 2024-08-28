using ActionServiceAPI.Application.DataTransferObjects.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Queries.GetActionById
{
    public class GetActionByIdQuery(int id) : IRequest<ActionDto?>
    {
        public int Id { get; init; } = id;
    }
}
