using Microsoft.EntityFrameworkCore;
using WarehouseServiceAPI.Infrastructure;
using WarehouseServiceAPI.Models;
using WarehouseServiceAPI.Utilities;

namespace WarehouseServiceAPI.Services
{
    public class PartService(WarehouseContext context) : IPartService
    {
        public async Task<DbActionResult> AddPart(Part part)
        {
            try
            {
                await context.AddAsync(part);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbActionResult(false, ex);
            }

            return new DbActionResult(true);
        }

        public async Task<DbActionResult> DeletePart(int id)
        {
            try
            {
                var part = await context.Parts.FindAsync(id);

                if (part is null)
                    return new DbActionResult(false, new Exception("Part not found!"));

                context.Parts.Remove(part);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbActionResult(false, ex);
            }

            return new DbActionResult(true);
        }

        public async Task<IEnumerable<Part>> GetAllParts() => await context.Parts.ToArrayAsync();

        public async Task<Part?> GetPartById(int id) => await context.Parts.FindAsync(id);

        public async Task<DbActionResult> UpdatePart(Part part)
        {
            try
            {
                var existingPart = await context.Parts.FindAsync(part.Id);

                if (existingPart is null)
                    return new DbActionResult(false, new Exception("Part not found!"));

                foreach (var prop in typeof(Part).GetProperties())
                    if (prop.Name != "Id")
                        prop.SetValue(existingPart, prop.GetValue(part));

                context.Parts.Update(existingPart);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DbActionResult(false, ex);
            }

            return new DbActionResult(true);
        }
    }
}
