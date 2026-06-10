using Microsoft.EntityFrameworkCore;
using Shoofly.Data.Entities.Identity;
using Shoofly.Infrastructure.Abstracts;
using Shoofly.Infrastructure.Data;
using Shoofly.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        #region Fields / Properties
        private readonly DbSet<UserRefreshToken> _userRefreshTokens;
        #endregion

        #region Constructor(s)
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            _userRefreshTokens = context.Set<UserRefreshToken>();
        }
        #endregion

        #region Methods
        #endregion
    }
}
