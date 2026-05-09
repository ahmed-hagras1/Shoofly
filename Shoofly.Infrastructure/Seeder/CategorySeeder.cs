using Shoofly.Data.Entities;
using Shoofly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Technical" },
                    new Category { Name = "Manual" } // For Fikedmitkm style physical services
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}