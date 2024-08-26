using ActionServiceAPI.Application.Action.Commands.Common;
using ActionServiceAPI.Domain.Models;
using MediatR;
using System.Collections.ObjectModel;

namespace ActionServiceAPI.Application.Action.Commands.UpdateActionCommand
{
    public class UpdateActionCommand(int id, string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<UsedPart> parts)
        : ActionCommandBase(name, description, startDate, endDate, createdBy, conductedBy, parts), IRequest<bool>
    {
        public int Id { get; init; } = id;

        List<UsedPart> NewUsedParts = [];

        List<UsedPart> ReturnedParts = [];

        public void SetUsedPartsList(List<UsedPart> usedParts) => NewUsedParts = usedParts;

        public void SetReturnedPartsList(List<UsedPart> returnedParts) => ReturnedParts = returnedParts;

        public ReadOnlyCollection<UsedPart> GetNewUsedPartsList() => NewUsedParts.AsReadOnly();

        public ReadOnlyCollection<UsedPart> GetReturnedPartsList() => ReturnedParts.AsReadOnly();
    }
}
