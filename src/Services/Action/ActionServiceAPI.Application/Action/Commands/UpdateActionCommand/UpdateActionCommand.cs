using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using MediatR;
using System.Collections.ObjectModel;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommand(int id, string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<SparePartDto> parts)
        : ActionCommandBase(name, description, startDate, endDate, createdBy, conductedBy, parts), IRequest<bool>
    {
        public int Id { get; init; } = id;

        List<SparePartDto> NewUsedParts = [];

        List<SparePartDto> ReturnedParts = [];

        public void SetUsedPartsList(List<SparePartDto> usedParts) => NewUsedParts = usedParts;

        public void SetReturnedPartsList(List<SparePartDto> returnedParts) => ReturnedParts = returnedParts;

        public ReadOnlyCollection<SparePartDto> GetNewUsedPartsList() => NewUsedParts.AsReadOnly();

        public ReadOnlyCollection<SparePartDto> GetReturnedPartsList() => ReturnedParts.AsReadOnly();
    }
}
