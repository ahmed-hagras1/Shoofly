using Microsoft.AspNetCore.Identity;
using Shoofly.Data.Entities.Identity;
using Shoofly.Data.Entities;
using Shoofly.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            // Only seed if no users exist
            if (!userManager.Users.Any())
            {
                // Grab the first country from the database to satisfy the CountryId FK
                var defaultCountry = dbContext.Set<Country>().FirstOrDefault();
                int defaultCountryId = defaultCountry != null ? defaultCountry.Id : 1;

                var defaultAdmin = new ApplicationUser
                {
                    UserName = "admin@shoofly.com",
                    Email = "admin@shoofly.com",
                    FullName = "Shoofly Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PreferredLanguage = "ar-EG",
                    CountryId = defaultCountryId
                };

                // Create the user with a strong default password
                var result = await userManager.CreateAsync(defaultAdmin, "Admin@123");

                // If successful, attach the Admin role to this user
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultAdmin, "Admin");
                }
            }
        }
    }
}