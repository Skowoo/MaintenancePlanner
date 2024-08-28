using ActionServiceAPI.Domain.Models;

namespace ActionServiceAPI.Application.DataTransferObjects.Models
{
    public class ActionDto(
        int id,
        string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        DateTime createdAt,
        Employee createdBy,
        Employee? conductedBy,
        List<SparePartDto> parts)
    {
        public int Id { get; init; } = id;

        public string Name { get; init; } = name;

        public string Description { get; init; } = description;

        public DateTime StartDate { get; init; } = startDate;

        public DateTime EndDate { get; init; } = endDate;

        public DateTime CreatedAt { get; init; } = createdAt;

        public string CreatedBy { get; init; } = createdBy.UserId;

        public string? ConductedBy { get; init; } = conductedBy?.UserId;

        public List<SparePartDto> Parts { get; init; } = parts;
    }
}
