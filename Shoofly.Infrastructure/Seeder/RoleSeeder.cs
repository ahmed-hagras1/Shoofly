using Microsoft.AspNetCore.Identity;
using Shoofly.Data.Entities.Identity;
using Shoofly.Shared.Security;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            // Updated: ServiceProvider → ManualServiceProvider + DigitalServiceProvider
            var systemRoles = new List<string>
            {
                "Admin",
                "Coordinator",
                "ManualServiceProvider",
                "DigitalServiceProvider",
                "Client"
            };

            foreach (var roleName in systemRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            // Seed all permission claims onto the Admin role
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var existingClaims = await roleManager.GetClaimsAsync(adminRole);
                var existingClaimValues = existingClaims.Select(c => c.Value).ToList();

                var permissionClasses = typeof(Permissions).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

                foreach (var module in permissionClasses)
                {
                    var permissions = module
                        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                        .Select(fi => fi.GetRawConstantValue()?.ToString());

                    foreach (var permission in permissions)
                    {
                        if (permission != null && !existingClaimValues.Contains(permission))
                        {
                            await roleManager.AddClaimAsync(adminRole, new Claim(Permissions.Type, permission));
                        }
                    }
                }
            }
        }
    }
}