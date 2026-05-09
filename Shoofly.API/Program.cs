using Shoofly.Infrastructure.Dependencies;
using Shoofly.Service.Dependencies;
using Shoofly.Core.Dependencies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

// SEEDING NAMESPACES
using Shoofly.Infrastructure.Data;
using Shoofly.Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity;
using Shoofly.Data.Entities.Identity;

// Add the Middleware namespace
using Shoofly.Api.Middlewares;

namespace Shoofly.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            try
            {
                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                #region Dependency injection.
                builder.Services.AddInfrastructureDependencies(builder.Configuration)
                    .AddServiceDependencies()
                    .AddCoreDependencies()
                    .AddIdentityDependencies(builder.Configuration);
                #endregion

                #region Localization Services
                builder.Services.AddLocalization();
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG")
                };

                builder.Services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new RequestCulture("ar-EG");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.ApplyCurrentCultureToResponseHeaders = true;
                });
                #endregion

                var app = builder.Build();

                //  ERROR HANDLER (MUST BE FIRST)
                app.UseMiddleware<ErrorHandlerMiddleware>();

                app.UseRequestLocalization();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                // AUTHENTICATION
                app.UseAuthentication(); // <-- CRITICAL: Must be added before Authorization
                app.UseAuthorization();

                app.MapControllers();

                // SEEDING BLOCK
                #region Seeding Database
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var dbContext = services.GetRequiredService<AppDbContext>();
                        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                        // ---------------------------------------------------
                        // STEP 1: Independent Tables (No Foreign Keys)
                        // ---------------------------------------------------
                        await CountrySeeder.SeedAsync(dbContext);
                        await CategorySeeder.SeedAsync(dbContext);
                        await RoleSeeder.SeedAsync(roleManager); // Roles don't depend on users

                        // ---------------------------------------------------
                        // STEP 2: Level 1 Dependencies
                        // ---------------------------------------------------
                        // SubCategories require Categories to exist first
                        await SubCategorySeeder.SeedAsync(dbContext);

                        // Users require Roles to exist first (so we can assign them)
                        await UserSeeder.SeedAsync(userManager, dbContext);

                        // ---------------------------------------------------
                        // STEP 3: Level 2 Dependencies
                        // ---------------------------------------------------
                        // Services require SubCategories to exist first
                        await ServiceSeeder.SeedAsync(dbContext);
                    }
                    catch (Exception ex)
                    {
                        // If seeding fails, log it to the console so we can see why
                        Console.WriteLine($"An error occurred during database seeding: {ex.Message}");
                    }
                }
                #endregion

                // Run the application
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}