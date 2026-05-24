using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoofly.Data.Entities.Identity;
using Shoofly.Service.Abstracts;
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
        #endregion
        #region Constractor
        // _userManager omitted for brevity if not used in this specific method

        public AuthorizationService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        #endregion
        #region Methods
        public async Task<List<ApplicationRole>> GetRolesListAsync(CancellationToken cancellationToken)
        {
            // Fetch all roles from the database asynchronously
            return await _roleManager.Roles.ToListAsync(cancellationToken);
        }
        #endregion
    }
}
