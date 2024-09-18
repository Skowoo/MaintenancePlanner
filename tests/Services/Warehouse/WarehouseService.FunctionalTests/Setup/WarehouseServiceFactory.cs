using CommonTestAssets.MockObjects;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WarehouseServiceAPI;
using WarehouseServiceAPI.Infrastructure;

namespace WarehouseService.FunctionalTests.Setup
{
    public class WarehouseServiceFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var originalContext = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WarehouseContext>));
                services.Remove(originalContext!);

                var dbConnection = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                services.Remove(dbConnection!);

                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<WarehouseContext>((container, options) =>
                    {
                        options.UseInMemoryDatabase("TestDatabase")
                        .UseInternalServiceProvider(container);
                    });

                var originalEventBus = services.SingleOrDefault(x => x.ServiceType == typeof(IEventBus));
                services.Remove(originalEventBus!);

                services.AddSingleton<IEventBus, EventBusMock>();
            });

            builder.UseEnvironment("Testing");
        }
    }
}
