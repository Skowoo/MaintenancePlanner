using EventBus.Abstractions;
using EventBusRabbitMQ;
using JwtGlobalConfiguration;
using Microsoft.EntityFrameworkCore;
using WarehouseServiceAPI.Infrastructure;
using WarehouseServiceAPI.IntegrationEvents;
using WarehouseServiceAPI.IntegrationEvents.EventHandlers;
using WarehouseServiceAPI.IntegrationEvents.Events;
using WarehouseServiceAPI.Services;

namespace WarehouseServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("WarehouseDb")
                ?? throw new InvalidOperationException("Connection string not found in configuration file!");

            builder.Services.AddDbContext<WarehouseContext>(
                options => options.UseSqlServer(connectionString));

            builder.Services.AddRabbitMQEventBus();

            builder.Services.AddScoped<IIntegrationEventService, IntegrationEventService>();
            builder.Services.AddScoped<IPartService, PartService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCommonJwtConfiguration();

            var app = builder.Build();

            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<SparePartsUsedInActionIntegrationEvent, SparePartsUsedInActionIntegrationEventHandler>();
            eventBus.Subscribe<SparePartsReturnedFromActionIntegrationEvent, SparePartsReturnedFromActionIntegrationEventHandler>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.SeedDatabase();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.UseAuthentication();

            app.Run();
        }
    }
}
