using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Infrastructure
{
    public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext(options)
    {
    }
}
