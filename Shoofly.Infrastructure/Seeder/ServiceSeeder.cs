using Shoofly.Data.Entities;
using Shoofly.Shared.Enums;
using Shoofly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class ServiceSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.Set<Service>().AnyAsync())
            {
                // Fetch Technical SubCategories
                var webDev = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Web Development");
                var mobileDev = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Mobile Development");
                var graphicDesign = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Graphic Design");

                // Fetch Manual SubCategories
                var plumbing = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Plumbing");
                var electrical = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Electrical");
                var acMaintenance = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "AC Maintenance");

                var services = new List<Service>();

                // --- TECHNICAL SERVICES (Remote / No Physical Attendance) ---
                if (webDev != null)
                {
                    services.Add(new Service { Title = "Create an Educational Platform", SubCategoryId = webDev.Id, RequiresPhysicalAttendance = false, ServiceAmountStart = 5000 });
                    services.Add(new Service { Title = "E-Commerce Website Setup", SubCategoryId = webDev.Id, RequiresPhysicalAttendance = false, ServiceAmountStart = 3000 });
                }
                if (mobileDev != null)
                {
                    services.Add(new Service { Title = "Create your Gym App", SubCategoryId = mobileDev.Id, RequiresPhysicalAttendance = false, ServiceAmountStart = 8000 });
                    services.Add(new Service { Title = "Delivery & Tracking App", SubCategoryId = mobileDev.Id, RequiresPhysicalAttendance = false, ServiceAmountStart = 10000 });
                }
                if (graphicDesign != null)
                {
                    // Maybe graphic design is hourly!
                    services.Add(new Service { Title = "Company Logo Design", SubCategoryId = graphicDesign.Id, RequiresPhysicalAttendance = false, HourlyRateStart = 50 });
                }

                // --- MANUAL SERVICES (Requires Physical Attendance) ---
                if (plumbing != null)
                {
                    services.Add(new Service { Title = "Fix Leaking Pipe", SubCategoryId = plumbing.Id, RequiresPhysicalAttendance = true, ServiceAmountStart = 150 });
                    services.Add(new Service { Title = "Install New Faucet or Sink", SubCategoryId = plumbing.Id, RequiresPhysicalAttendance = true, ServiceAmountStart = 300 });
                }
                if (electrical != null)
                {
                    services.Add(new Service { Title = "Install Lighting Fixtures", SubCategoryId = electrical.Id, RequiresPhysicalAttendance = true, HourlyRateStart = 100 });
                    services.Add(new Service { Title = "Fix Short Circuit", SubCategoryId = electrical.Id, RequiresPhysicalAttendance = true, ServiceAmountStart = 200 });
                }
                if (acMaintenance != null)
                {
                    services.Add(new Service { Title = "Split AC Deep Cleaning", SubCategoryId = acMaintenance.Id, RequiresPhysicalAttendance = true, ServiceAmountStart = 400 });
                }

                if (services.Count > 0)
                {
                    // Note: I left out PricingType because I don't know the exact values in your enum. 
                    // If you want to seed it, just add `PricingType = PricingType.Fixed` (or whatever your enum values are named) to each object above!

                    await dbContext.Set<Service>().AddRangeAsync(services);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}