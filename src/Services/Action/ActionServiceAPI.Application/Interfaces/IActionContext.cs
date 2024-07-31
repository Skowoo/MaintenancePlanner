using ActionServiceAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Application.Interfaces
{
    public interface IActionContext
    {
        public DbSet<ActionEntity> Actions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<UsedPart> UsedParts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
