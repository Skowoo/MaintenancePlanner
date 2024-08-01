using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Queries.GetActionById
{
    public class GetActionByIdQuery(int id) : IRequest<ActionEntity>
    {
        public int Id { get; init; } = id;
    }
}
