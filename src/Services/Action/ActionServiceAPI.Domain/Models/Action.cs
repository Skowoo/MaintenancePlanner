using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    public class Action(string name, string description, DateTime startDate, DateTime endDate, string createdBy) : Entity
    {
        public string Name { get; set; } = name;

        public string Description { get; set; } = description;

        public DateTime StartDate { get; set; } = startDate;

        public DateTime EndDate { get; set; } = endDate;

        public DateTime CreatedAt { get; init; } = DateTime.Now;

        public string CreatedById { get; set; } = createdBy;

        public string? MadeById { get; set; } 

        public IEnumerable<int> Parts { get; set; } = [];
    }
}
