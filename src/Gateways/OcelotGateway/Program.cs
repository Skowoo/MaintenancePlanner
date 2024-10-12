using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotGateway.OcelotConfiguration;

namespace OcelotGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Configuration.AddOcelotWithSwaggerSupport(OcelotOptions.ConfigureOcelotWithSwaggerOptions);

            builder.Services.AddOcelot(builder.Configuration)
                .AddAppConfiguration();

            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerForOcelotUI();
            }

            app.UseOcelot().Wait();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
