using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoofly.Data.Entities.Identity;
using Shoofly.Infrastructure.Data;
using Shoofly.Service.Abstracts;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Service.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        #region Constractor
        // _userManager omitted for brevity if not used in this specific method

        public AuthorizationService(RoleManager<ApplicationRole> roleManager,
            AppDbContext appDbContext,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        #endregion
        #region Methods
        public async Task<List<ApplicationRole>> GetRolesListAsync(CancellationToken cancellationToken)
        {
            return await _roleManager.Roles.ToListAsync(cancellationToken);
        }
        public async Task<ApplicationRole?> GetRoleByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _roleManager.Roles.FirstOrDefaultAsync( r =>  r.Id == id, cancellationToken);
        }
        public async Task<string> AddRoleAsync(string roleName)
        {
            // 1. Check if the role already exists
            var isExist = await _roleManager.RoleExistsAsync(roleName);
            if (isExist)
            {
                return "RoleIsExist";
            }

            // 2. Create the new role
            var identityRole = new ApplicationRole
            {
                Name = roleName
            };

            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return "Success";
            }

            return "Failed";
        }
        public async Task<string> EditRoleAsync(string id, string newName)
        {
            // Check if the role exists
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return "NotFound";
            }

            // Update the name
            role.Name = newName;

            // Save changes to the database
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return "Success";
            }

            return "Failed";
        }
        public async Task<string> DeleteRoleAsync(string roleId)
        {
            // 1. Check if the role actually exists
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return "NotFound";

            var usersInRole = await _appDbContext.UserRoles.AnyAsync(ur => ur.RoleId ==  roleId);

            if (usersInRole)
            {
                return "HasUsers";
            }

            // Safe to delete
            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded ? "Success" : "Failed";
        }
        public async Task<(List<ApplicationRole> Roles, IList<string> UserRoles)?> GetManageUserRolesDataAsync(string userId)
        {
            // Check if user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            // Get ALL roles in the system
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Get the roles assigned to this specific user (returns a list of Role Names)
            var userRoles = await _userManager.GetRolesAsync(user);

            // Return them as a raw Tuple to the Core layer
            return (allRoles, userRoles);
        }
        public async Task<string> UpdateUserRolesAsync(string userId, List<(string RoleName, bool HasRole)> userRoles)
        {
            // Find the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "UserNotFound";

            // Get the user's CURRENT roles from the database
            var currentRoles = await _userManager.GetRolesAsync(user);

            // 3. Find roles to ADD 
            var rolesToAdd = userRoles
                .Where(x => x.HasRole && !currentRoles.Contains(x.RoleName))
                .Select(x => x.RoleName)
                .ToList();

            // Find roles to REMOVE
            var rolesToRemove = userRoles
                .Where(x => !x.HasRole && currentRoles.Contains(x.RoleName))
                .Select(x => x.RoleName)
                .ToList();

            // START THE TRANSACTION
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                int num1 = 0, num2 = 0;
                //int x = num1 / num2;

                // Execute Removals
                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                    if (!removeResult.Succeeded)
                    {
                        // No need to rollback yet, nothing was added, but we exit safely
                        await transaction.RollbackAsync();
                        return "FailedToRemoveOldRoles";
                    }
                }

                // int x = num1 / num2;

                // 3. Execute Additions
                if (rolesToAdd.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

                    

                    if (!addResult.Succeeded)
                    {
                        // We MUST undo the removals so the user isn't left broken.
                        await transaction.RollbackAsync();
                        return "FailedToAddNewRoles";
                    }
                }
                // int x = num1 / num2; 

                // EVERYTHING SUCCEEDED! Commit the changes permanently to the database.
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception)
            {
                // If the server crashes, database connection drops, or an exception is thrown,
                // this guarantees the database undoes any partial work.
                await transaction.RollbackAsync();
                return "FailedToUpdateRoles"; // You can add this to your localization keys!
            }
        }
        #endregion
    }
}
