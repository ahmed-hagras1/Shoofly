using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Service.Abstracts
{
    public interface IAuthorizationService
    {
        Task<List<ApplicationRole>> GetRolesListAsync(CancellationToken cancellationToken);
        Task<ApplicationRole?> GetRoleByIdAsync(string id, CancellationToken cancellationToken);
        Task<string> AddRoleAsync(string roleName);
        Task<string> EditRoleAsync(string id, string newName);
        Task<string> DeleteRoleAsync(string roleId);

        // Returns a Tuple containing all system roles AND the specific user's roles
        Task<(List<ApplicationRole> Roles, IList<string> UserRoles)?> GetManageUserRolesDataAsync(string userId);
        // Accepts a basic Tuple (String, Boolean) to maintain zero Core dependencies
        Task<string> UpdateUserRolesAsync(string userId, List<(string RoleName, bool HasRole)> userRoles);
    }
}
