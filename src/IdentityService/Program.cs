using IdentityServiceAPI.Infrastructure;
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
                options => options.UseInMemoryDatabase("MemoIdentityDb"));

            // Default Identity Endpoints Registration
            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            var app = builder.Build();

            // Map default Identity Endpoints
            app.MapGroup("api/v1")
               .MapIdentityApi<IdentityUser>();

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
