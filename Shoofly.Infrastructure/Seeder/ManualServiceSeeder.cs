using Microsoft.EntityFrameworkCore;
using Shoofly.Data.Entities;
using Shoofly.Infrastructure.Data;
using Shoofly.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    /// <summary>
    /// Seeds ManualService records only.
    /// Technical sub-categories (Web Dev, Mobile, etc.) have NO seeded services —
    /// they are populated dynamically by DigitalServiceProvider registrations.
    /// </summary>
    public static class ManualServiceSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.ManualServices.AnyAsync())
            {
                var plumbing = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Plumbing");
                var electrical = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Electrical");
                var cleaning = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Cleaning Services");
                var ac = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "AC Maintenance");
                var carpentry = await dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Name == "Carpentry");

                var services = new List<ManualService>();

                if (plumbing != null)
                {
                    services.Add(new ManualService
                    {
                        Title = "Fix Leaking Pipe",
                        SubCategoryId = plumbing.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 150,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                    services.Add(new ManualService
                    {
                        Title = "Install New Faucet or Sink",
                        SubCategoryId = plumbing.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 300,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                    services.Add(new ManualService
                    {
                        Title = "Water Heater Installation",
                        SubCategoryId = plumbing.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 500,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                }

                if (electrical != null)
                {
                    services.Add(new ManualService
                    {
                        Title = "Install Lighting Fixtures",
                        SubCategoryId = electrical.Id,
                        PricingType = PricingType.PerHour,
                        HourlyRateStart = 100,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                    services.Add(new ManualService
                    {
                        Title = "Fix Short Circuit",
                        SubCategoryId = electrical.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 200,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                }

                if (cleaning != null)
                {
                    services.Add(new ManualService
                    {
                        Title = "Standard Room Cleaning",
                        SubCategoryId = cleaning.Id,
                        PricingType = PricingType.PerHour,
                        HourlyRateStart = 80,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                    services.Add(new ManualService
                    {
                        Title = "Full-Home Deep Clean",
                        Description = "Requires a cleaning team. Covers all rooms, kitchen, and bathrooms.",
                        SubCategoryId = cleaning.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 1200,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = true   // Must be assigned to a ManualTeam
                    });
                }

                if (ac != null)
                {
                    services.Add(new ManualService
                    {
                        Title = "Split AC Deep Cleaning",
                        SubCategoryId = ac.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 400,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                    services.Add(new ManualService
                    {
                        Title = "AC Gas Refill",
                        SubCategoryId = ac.Id,
                        PricingType = PricingType.FixedAmount,
                        ServiceAmountStart = 350,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                }

                if (carpentry != null)
                {
                    services.Add(new ManualService
                    {
                        Title = "Furniture Assembly",
                        SubCategoryId = carpentry.Id,
                        PricingType = PricingType.PerHour,
                        HourlyRateStart = 120,
                        RequiresPhysicalAttendance = true,
                        RequiresTeam = false
                    });
                }

                if (services.Count > 0)
                {
                    await dbContext.ManualServices.AddRangeAsync(services);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
