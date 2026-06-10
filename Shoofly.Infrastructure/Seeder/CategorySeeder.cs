using Shoofly.Data.Entities;
using Shoofly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoofly.Shared.Enums;

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
                    new Category
                    {
                        Name = "Manual",
                        Type = CategoryType.Manual,
                        IconUrl = null  // Set a real icon later
                    },
                    new Category
                    {
                        Name = "Technical",
                        Type = CategoryType.Technical,
                        IconUrl = null
                    }
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}