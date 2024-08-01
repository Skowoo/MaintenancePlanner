using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Commands.UpdateActionCommand
{
    public class UpdateActionCommand : IRequest<bool>
    {
        public UpdateActionCommand(int id, string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<UsedPart> parts)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            CreatedBy = createdBy;
            ConductedBy = conductedBy;
            Parts = parts;
        }

        public int Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public string CreatedBy { get; init; }

        public string? ConductedBy { get; init; }

        public IEnumerable<UsedPart> Parts { get; init; }
    }
}
