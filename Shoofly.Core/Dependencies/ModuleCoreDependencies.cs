using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shoofly.Core.Behaviors;
using System.Reflection;
using FluentValidation;

namespace Shoofly.Core.Dependencies
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            // 1. Register MediatR and add the ValidationBehavior
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // 2. Register all your FluentValidation rules automatically
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
