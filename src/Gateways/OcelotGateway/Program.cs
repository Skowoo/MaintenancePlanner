using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

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

            builder.Configuration.AddOcelotWithSwaggerSupport(options =>
            {
                options.Folder = "OcelotConfiguration";
            });
            builder.Services.AddOcelot(builder.Configuration);
            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });
            app.UseOcelot().Wait();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
