using ActionServiceAPI.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace ActionServiceAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            });

            return services;
        }
    }
}
