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

            builder.Configuration.AddOcelot("OcelotConfiguration", builder.Environment);
            builder.Services.AddOcelot(builder.Configuration);

            builder.Configuration.AddJsonFile("OcelotConfiguration/ocelot.swagger.json", optional: true);
            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseOcelot();

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.DownstreamSwaggerHeaders = [new KeyValuePair<string, string>("Auth-Key", "AuthValue")];
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
