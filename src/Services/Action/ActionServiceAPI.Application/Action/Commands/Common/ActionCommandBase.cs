using ActionServiceAPI.Application.DataTransferObjects.Models;

namespace ActionServiceAPI.Application.Action.Commands.Common
{
    public abstract class ActionCommandBase(string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<SparePartDto> parts)
    {
        public string Name { get; init; } = name;

        public string Description { get; init; } = description;

        public DateTime StartDate { get; init; } = startDate;

        public DateTime EndDate { get; init; } = endDate;

        public string CreatedBy { get; init; } = createdBy;

        public string? ConductedBy { get; init; } = conductedBy;

        public IEnumerable<SparePartDto> Parts { get; init; } = parts;
    }
}
