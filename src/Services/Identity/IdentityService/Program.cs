using EventBusRabbitMQ;
using IdentityServiceAPI.Infrastructure;
using IdentityServiceAPI.IntegrationEvents;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer("Server=sqlserver;Database=IdentityDb;User Id=sa;Password=P@ssw0rd112345678;MultipleActiveResultSets=true;TrustServerCertificate=true"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddTransient<IIntegrationEventService, IntegrationEventService>();
            builder.Services.AddRabbitMQEventBus();

            var app = builder.Build();

            app.SeedDatabase();

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
