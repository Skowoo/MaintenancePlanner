using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommand(int id, string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<UsedPart> parts)
        : ActionCommandBase(name, description, startDate, endDate, createdBy, conductedBy, parts), IRequest<bool>
    {
        public int Id { get; init; } = id;
    }
}
