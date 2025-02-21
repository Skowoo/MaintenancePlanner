using EventBusRabbitMQ;
using IdentityServiceAPI.GrpcServices;
using IdentityServiceAPI.Infrastructure;
using IdentityServiceAPI.IntegrationEvents;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using JwtGlobalConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddGrpc();

            var connectionString = builder.Configuration.GetConnectionString("IdentityDb")
                ?? throw new InvalidOperationException("Connection string not found in configuration file!");

            builder.Services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddTransient<IIntegrationEventService, IntegrationEventService>();
            builder.Services.AddRabbitMQEventBus();

            builder.Services.AddCommonJwtConfiguration();

            var app = builder.Build();

            app.MapGrpcService<LoginConfirmationGrpcService>();

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
