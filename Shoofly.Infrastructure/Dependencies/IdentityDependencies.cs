using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoofly.Data.Entities.Identity;
using Shoofly.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Dependencies
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddIdentityDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // 2. Add ASP.NET Core Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // --- Password Settings (Facebook Style) ---
                // Low friction: Only enforce length and basic characters.
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true; // Recommend keeping numbers for basic security

                // Disable frustrating requirements
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false; // No forced symbols
                options.Password.RequiredUniqueChars = 1;

                // --- Lockout Settings (Crucial for security) ---
                // Because the password is easier to type, you MUST protect against brute-force attacks.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // Lock them out for 10 mins
                options.Lockout.MaxFailedAccessAttempts = 5; // After 5 wrong guesses
                options.Lockout.AllowedForNewUsers = true;

                // --- User Settings ---
                // Ensure every user has a unique email
                // Note ==> If you using phone number for registration, you can set this to false and use phone number as the unique identifier instead.
                // options.User.RequireUniqueEmail = true;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>() // Tells Identity to save users in your DB
            .AddDefaultTokenProviders(); // Required for password resets/email confirmation

            return services;
        }
    }
}
