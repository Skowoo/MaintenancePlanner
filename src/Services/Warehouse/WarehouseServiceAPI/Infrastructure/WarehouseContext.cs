using Microsoft.EntityFrameworkCore;
using WarehouseServiceAPI.Models;

namespace WarehouseServiceAPI.Infrastructure
{
    public class WarehouseContext(DbContextOptions<WarehouseContext> options) : DbContext(options)
    {
        public DbSet<Part> Parts { get; set; }
    }
}
