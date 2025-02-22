using ActionServiceAPI.Application;
using ActionServiceAPI.Application.IntegrationEvents;
using ActionServiceAPI.Application.IntegrationEvents.EventHandling;
using ActionServiceAPI.Application.IntegrationEvents.Events;
using ActionServiceAPI.Infrastructure;
using ActionServiceAPI.Infrastructure.Data;
using ActionServiceAPI.Web.Middleware;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using JwtGlobalConfiguration;

namespace ActionServiceAPI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegisterInfrastructureServices(builder.Configuration);
            builder.Services.RegisterApplicationServices();

            builder.Services.AddTransient<IIntegrationEventService, IntegrationEventService>();

            builder.Services.AddRabbitMQEventBus();

            builder.Services.AddCommonJwtConfiguration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.Services.SeedDatabase();
            }

            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NewPartAddedIntegrationEvent, NewPartAddedIntegrationEventHandler>();
            eventBus.Subscribe<NewUserCreatedIntegrationEvent, NewUserCreatedIntegrationEventHandler>();

            app.UseAuthorization();

            app.UseMiddleware<DomainExceptionHandlingMiddleware>();

            app.MapControllers();

            app.UseAuthentication();

            app.Run();
        }
    }
}
