using WarehouseServiceAPI.Models.SeedWork;

namespace WarehouseServiceAPI.Models
{
    public class Part(string name, string description, string manufacturer, string model) : Entity
    {
        public string Name { get; set; } = name;

        public string Description { get; set; } = description;

        public string Manufacturer { get; set; } = manufacturer;

        public string Model { get; set; } = model;

        public int QuantityOnStock { get; set; }

        public int MinimumStock { get; set; }
    }
}
