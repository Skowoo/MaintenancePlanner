using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    public abstract class SparePart : Entity
    {
        public int PartId { get; set; }

        public int Quantity { get; set; }
    }
}
