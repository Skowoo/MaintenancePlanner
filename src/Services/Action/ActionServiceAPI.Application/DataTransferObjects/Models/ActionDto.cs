namespace ActionServiceAPI.Application.DataTransferObjects.Models
{
    public class ActionDto
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public DateTime CreatedAt { get; init; }

        public string CreatedBy { get; init; } = string.Empty;

        public string? ConductedBy { get; init; }

        public List<SparePartDto> Parts { get; init; } = [];
    }
}
