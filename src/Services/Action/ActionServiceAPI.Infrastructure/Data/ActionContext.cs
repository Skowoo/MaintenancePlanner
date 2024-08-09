using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Infrastructure.Data
{
    public class ActionContext(DbContextOptions<ActionContext> options) : DbContext(options), IActionContext
    {
        public DbSet<ActionEntity> Actions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<UsedPart> UsedParts { get; set; }

        public DbSet<AvailablePart> AvailableParts { get; set; }
    }
}