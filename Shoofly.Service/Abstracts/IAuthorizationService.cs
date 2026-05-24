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
    }
}
