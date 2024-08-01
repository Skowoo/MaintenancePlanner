using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    public class UsedPart(int partId, int quantity) : Entity
    {
        // Refactor
        protected UsedPart() : this(0, 0) { }

        public int PartId { get; set; } = partId;

        public int Quantity { get; set; } = quantity;
    }
}
