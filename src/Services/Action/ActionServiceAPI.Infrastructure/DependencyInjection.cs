using ActionServiceAPI.Application.Interfaces.DataRepositories;
using ActionServiceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActionServiceAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ActionDb")
                ?? throw new InvalidOperationException("Connection string not found in configuration file!");

            services.AddDbContext<ActionContext>(
                options => options.UseSqlServer(connectionString));

            services.AddScoped<IActionContext, ActionContext>();

            return services;
        }
    }
}
