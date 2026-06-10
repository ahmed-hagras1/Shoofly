using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Abstracts
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {
    }
}
