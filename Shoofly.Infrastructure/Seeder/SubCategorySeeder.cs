using Shoofly.Data.Entities;
using Shoofly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class SubCategorySeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.SubCategories.AnyAsync())
            {
                var technical = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Technical");
                var manual = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Manual");

                var subCategories = new List<SubCategory>();

                // --- TECHNICAL SUB-CATEGORIES ---
                if (technical != null)
                {
                    subCategories.Add(new SubCategory { Name = "Web Development", CategoryId = technical.Id });
                    subCategories.Add(new SubCategory { Name = "Mobile Development", CategoryId = technical.Id });
                    subCategories.Add(new SubCategory { Name = "Graphic Design", CategoryId = technical.Id });
                    subCategories.Add(new SubCategory { Name = "Digital Marketing", CategoryId = technical.Id });
                }

                // --- MANUAL SUB-CATEGORIES ---
                if (manual != null)
                {
                    subCategories.Add(new SubCategory { Name = "Plumbing", CategoryId = manual.Id });
                    subCategories.Add(new SubCategory { Name = "Electrical", CategoryId = manual.Id });
                    subCategories.Add(new SubCategory { Name = "Carpentry", CategoryId = manual.Id });
                    subCategories.Add(new SubCategory { Name = "Cleaning Services", CategoryId = manual.Id });
                    subCategories.Add(new SubCategory { Name = "AC Maintenance", CategoryId = manual.Id });
                }

                if (subCategories.Count > 0)
                {
                    await dbContext.SubCategories.AddRangeAsync(subCategories);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}