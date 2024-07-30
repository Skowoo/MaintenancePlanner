using ActionServiceAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ActionServiceAPI.Infrastructure
{
    internal class ActionContext(DbContextOptions<ActionContext> options) : DbContext(options)
    {
        public DbSet<Domain.Models.Action> Actions { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
