using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    public class UsedPart : Entity
    {
        public int PartId { get; set; }

        public int Quantity { get; set; }
    }
}
