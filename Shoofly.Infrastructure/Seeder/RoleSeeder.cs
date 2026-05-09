using Microsoft.AspNetCore.Identity;
using Shoofly.Data.Entities.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            // Only seed if no roles exist
            if (!roleManager.Roles.Any())
            {
                var roles = new List<ApplicationRole>
                {
                    new ApplicationRole { Name = "Admin" },
                    new ApplicationRole { Name = "Coordinator" },
                    new ApplicationRole { Name = "ServiceProvider" },
                    new ApplicationRole { Name = "Client" }
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}