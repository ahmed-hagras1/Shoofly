using Microsoft.Extensions.DependencyInjection;
using Shoofly.Service.Abstracts;
using Shoofly.Service.Implementations;

namespace Shoofly.Service.Dependencies
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
