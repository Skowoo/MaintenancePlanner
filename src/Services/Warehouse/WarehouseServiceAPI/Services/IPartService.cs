using WarehouseServiceAPI.Models;
using WarehouseServiceAPI.Utilities;

namespace WarehouseServiceAPI.Services
{
    public interface IPartService
    {
        Task<Part?> GetPartById(int id);

        Task<IEnumerable<Part>> GetAllParts();

        Task<DbActionResult> AddPart(Part part);

        Task<DbActionResult> UpdatePart(Part part);

        Task<DbActionResult> DeletePart(int id);

        Task<DbActionResult> DecreasePartQuantity(int partId, int quantity);
    }
}
