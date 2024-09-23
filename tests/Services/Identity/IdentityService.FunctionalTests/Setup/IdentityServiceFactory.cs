using CommonTestAssets.MockObjects;
using EventBus.Abstractions;
using IdentityServiceAPI;
using IdentityServiceAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace IdentityService.FunctionalTests.Setup
{
    public class IdentityServiceFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var originalContextOptions = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<IdentityContext>));
                services.Remove(originalContextOptions!);

                var dbConnection = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                services.Remove(dbConnection!);

                services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<IdentityContext>((container, options) =>
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
