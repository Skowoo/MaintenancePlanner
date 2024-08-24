using WarehouseServiceAPI.Models;
using WarehouseServiceAPI.Utilities;

namespace WarehouseServiceAPI.Services
{
    public interface IPartService
    {
        Task<Part?> GetPartByIdAsync(int id);

        Task<IEnumerable<Part>> GetAllPartsAsync();

        Task<(DbActionResult Result, int? NewPartId)> AddPartAsync(Part part);

        Task<DbActionResult> UpdatePartAsync(Part part);

        Task<DbActionResult> DeletePartAsync(int id);

        Task<DbActionResult> DecreasePartQuantityAsync(int partId, int quantity);

        Task<DbActionResult> IncreasePartQuantityAsync(int partId, int quantity);
    }
}
