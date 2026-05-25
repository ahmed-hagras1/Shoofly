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
        #endregion
        #region Constractor
        // _userManager omitted for brevity if not used in this specific method

        public AuthorizationService(RoleManager<ApplicationRole> roleManager, AppDbContext appDbContext)
        {
            _roleManager = roleManager;
            _appDbContext = appDbContext;
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
        #endregion
    }
}
