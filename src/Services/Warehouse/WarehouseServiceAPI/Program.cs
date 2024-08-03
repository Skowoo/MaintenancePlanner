using EventBus.Abstractions;
using EventBusRabbitMQ;
using Microsoft.EntityFrameworkCore;
using WarehouseServiceAPI.Infrastructure;
using WarehouseServiceAPI.IntegrationEvents;
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
                options => options.UseInMemoryDatabase("WarehouseMemoryDb"));

            builder.Services.AddScoped<IPartService, PartService>(); // Refactor - check lifetimes
            builder.Services.AddSingleton<IIntegrationEventService, IntegrationEventService>();
            builder.Services.AddSingleton<IEventBus, EventBusService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
