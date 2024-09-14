using ActionServiceAPI.Infrastructure.Data;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ActionService.FunctionalTests.Setup
{
    public class ActionServiceFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var originalContext = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ActionContext>));
                services.Remove(originalContext!);

                var dbConnection = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                services.Remove(dbConnection!);

                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<ActionContext>((container, options) =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .UseInternalServiceProvider(container);
                    });

                var originalEventBus = services.SingleOrDefault(x => x.ServiceType == typeof(IEventBus));
                services.Remove(originalEventBus!);

                services.AddSingleton<IEventBus, EventBusMock>();
            });
        }
    }
}
