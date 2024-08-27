using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using MediatR;

namespace ActionServiceAPI.Application.Action.Commands.CreateActionCommand
{
    public class CreateActionCommand(string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<SparePartDto> parts)
        : ActionCommandBase(name, description, startDate, endDate, createdBy, conductedBy, parts), IRequest<int>
    {
    }
}
