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
                if (!userManager.Users.Any())
                {
                    var defaultCountry = dbContext.Set<Country>().FirstOrDefault();
                    int defaultCountryId = defaultCountry?.Id ?? 1;

                    // Admin is a plain ApplicationUser (not a Client/Provider subtype)
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

                    var result = await userManager.CreateAsync(defaultAdmin, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultAdmin, "Admin");
                    }
                }
            }
        }
    
}