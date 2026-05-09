using Shoofly.Data.Entities;
using Shoofly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class CountrySeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.Set<Country>().AnyAsync())
            {
                var countries = new List<Country>
                {
                    new Country { Name = "Egypt" }, // Based on your current location!
                    new Country { Name = "Saudi Arabia" },
                    new Country { Name = "United Arab Emirates" }
                };

                await dbContext.Set<Country>().AddRangeAsync(countries);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}