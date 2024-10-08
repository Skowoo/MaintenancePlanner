﻿using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    public class ActionEntity(string name, string description, DateTime startDate, DateTime endDate, Employee createdBy, Employee? conductedBy) : Entity
    {
        protected ActionEntity() : this("", "", DateTime.UtcNow, DateTime.UtcNow, new Employee(""), null) { }

        public string Name { get; set; } = name;

        public string Description { get; set; } = description;

        public DateTime StartDate { get; set; } = startDate;

        public DateTime EndDate { get; set; } = endDate;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public Employee CreatedBy { get; set; } = createdBy;
        public int CreatedById { get; set; } = createdBy.Id;

        public Employee? ConductedBy { get; set; } = conductedBy;
        public int? ConductedById { get; set; } = conductedBy?.Id;

        private readonly List<UsedPart> _usedParts = [];
        public IReadOnlyCollection<UsedPart> Parts => _usedParts;

        public void AddPart(UsedPart part) => _usedParts.Add(part);

        public void UpdatePartsList(IEnumerable<UsedPart> parts)
        {
            _usedParts.Clear();
            _usedParts.AddRange(parts);
        }
    }
}