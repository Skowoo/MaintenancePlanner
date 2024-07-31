using ActionServiceAPI.Application.Interfaces;
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
                => options.UseInMemoryDatabase("MemoActionDb"));

            services.AddScoped<IActionContext, ActionContext>();

            return services;
        }
    }
}
