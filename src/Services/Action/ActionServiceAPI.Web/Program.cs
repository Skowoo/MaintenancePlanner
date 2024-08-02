using ActionServiceAPI.Infrastructure;
using ActionServiceAPI.Application;
using ActionServiceAPI.Web.Middleware;

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

            builder.Services.RegisterInfrastructureServices();
            builder.Services.RegisterApplicationServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseMiddleware<DomainExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
