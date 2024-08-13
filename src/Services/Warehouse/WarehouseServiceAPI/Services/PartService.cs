using Microsoft.EntityFrameworkCore;
using WarehouseServiceAPI.Exceptions;
using WarehouseServiceAPI.Infrastructure;
using WarehouseServiceAPI.IntegrationEvents;
using WarehouseServiceAPI.IntegrationEvents.Events;
using WarehouseServiceAPI.Models;
using WarehouseServiceAPI.Utilities;

namespace WarehouseServiceAPI.Services
{
    public class PartService(
        WarehouseContext context,
        IIntegrationEventService integrationEventService,
        ILogger<PartService> logger) : IPartService
    {
        readonly WarehouseContext _context = context;
        readonly IIntegrationEventService _integrationEventService = integrationEventService;
        readonly ILogger<PartService> _logger = logger;

        public async Task<DbActionResult> AddPart(Part part)
        {
            try
            {
                await _context.AddAsync(part);
                await _context.SaveChangesAsync();

                _integrationEventService.PublishIntegrationEvent(new NewPartAddedIntegrationEvent(part.Id, part.QuantityOnStock));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding new part to the database.");
                return new DbActionResult(false, ex);
            }

            return new DbActionResult(true);
        }

        public async Task<DbActionResult> DecreasePartQuantity(int partId, int quantity)
        {
            var part = _context.Parts.SingleOrDefault(p => p.Id == partId);

            if (part is null)
                return new DbActionResult(false, new WarehouseDomainException("Part not found!"));

            if (!(part.QuantityOnStock - quantity >= 0))
                return new DbActionResult(false, new Exception("Not enough parts on stock!"));

            part.QuantityOnStock -= quantity;
            await _context.SaveChangesAsync();
            return new DbActionResult(true);
        }

        public async Task<DbActionResult> DeletePart(int id)
        {
            try
            {
                var part = await _context.Parts.FindAsync(id);

                if (part is null)
                    return new DbActionResult(false, new Exception("Part not found!"));

                _context.Parts.Remove(part);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting part from the database.");
                return new DbActionResult(false, ex);
            }

            return new DbActionResult(true);
        }

        public async Task<IEnumerable<Part>> GetAllParts() => await _context.Parts.ToArrayAsync();

        public async Task<Part?> GetPartById(int id) => await _context.Parts.FindAsync(id);

        public async Task<DbActionResult> UpdatePart(Part part)
        {
            try
            {
                var existingPart = await _context.Parts.FindAsync(part.Id);

                if (existingPart is null)
                    return new DbActionResult(false, new Exception("Part not found!"));

                foreach (var prop in typeof(Part).GetProperties())
                    if (prop.Name != "Id")
                        prop.SetValue(existingPart, prop.GetValue(part));

                _context.Parts.Update(existingPart);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating part in the database.");
                return new DbActionResult(false, ex);
            }

            return new DbActionResult(true);
        }
    }
}
