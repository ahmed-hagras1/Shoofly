using Shoofly.Data.Entities.Identity;
using Shoofly.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Service.Abstracts
{
    public interface IAuthenticationService
    {
        Task<JWTAuthResult> GetJWTToken(ApplicationUser user);
        Task<JWTAuthResult> GetRefreshToken(string accessToken, string refreshToken);
        Task<string> ValidateToken(string accessToken);
        Task<string> RevokeRefreshToken(string accessToken);
        Task<string> RevokeToken(string refreshToken);
        Task<string> RevokeAllSessions(string userId);
    }
}
