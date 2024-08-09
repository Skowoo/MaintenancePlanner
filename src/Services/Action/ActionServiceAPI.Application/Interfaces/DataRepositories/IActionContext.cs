using ActionServiceAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Interfaces.DataRepositories
{
    public interface IActionContext
    {
        public DbSet<ActionEntity> Actions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<UsedPart> UsedParts { get; set; }

        public DbSet<AvailablePart> AvailableParts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
