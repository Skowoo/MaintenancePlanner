using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActionServiceAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<ActionContext>(options
                => options.UseSqlServer("Server=sqlserver;Database=ActionDb;User Id=sa;Password=P@ssw0rd112345678;MultipleActiveResultSets=true;TrustServerCertificate=true"));

            services.AddScoped<IActionContext, ActionContext>();

            return services;
        }
    }
}
