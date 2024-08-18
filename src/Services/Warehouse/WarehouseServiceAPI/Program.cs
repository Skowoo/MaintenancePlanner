using EventBus.Abstractions;
using EventBusRabbitMQ;
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

            builder.Services.AddDbContext<WarehouseContext>(
                options => options.UseSqlServer("Server=sqlserver;Database=WarehouseDb;User Id=sa;Password=P@ssw0rd112345678;MultipleActiveResultSets=true;TrustServerCertificate=true"));

            builder.Services.AddRabbitMQEventBus();

            builder.Services.AddScoped<IIntegrationEventService, IntegrationEventService>();
            builder.Services.AddScoped<IPartService, PartService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<SparePartsUsedInActionIntegrationEvent, SparePartsUsedInActionIntegrationEventHandler>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.SeedDatabase();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
