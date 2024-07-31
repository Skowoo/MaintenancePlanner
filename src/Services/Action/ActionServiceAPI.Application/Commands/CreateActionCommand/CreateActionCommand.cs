using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Application.Commands.CreateActionCommand
{
    public class CreateActionCommand : IRequest
    {
        public CreateActionCommand(string name, string description, DateTime startDate, DateTime endDate, string createdBy, string conductedBy, IEnumerable<UsedPart> parts)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            CreatedBy = createdBy;
            ConductedBy = conductedBy;
            Parts = parts;
        }

        public string Name { get; init; }

        public string Description { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public string CreatedBy { get; init; }

        public string? ConductedBy { get; init; }

        public IEnumerable<UsedPart> Parts { get; init; }
    }
}
