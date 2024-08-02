using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Queries.GetActionById
{
    public class GetActionByIdQuery(int id) : IRequest<ActionEntity?>
    {
        public int Id { get; init; } = id;
    }
}
