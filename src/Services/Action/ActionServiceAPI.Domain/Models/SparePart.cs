using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    /// <summary>
    /// Abstract class to represent spare part
    /// </summary>
    public abstract class SparePart : Entity
    {
        /// <summary>
        /// PartId from WarehouseAPI
        /// </summary>
        public int PartId { get; set; }

        public int Quantity { get; set; }
    }
}
