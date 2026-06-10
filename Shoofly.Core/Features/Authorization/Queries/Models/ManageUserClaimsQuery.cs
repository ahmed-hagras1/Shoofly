using MediatR;
using Shoofly.Core.Bases;
using Shoofly.Core.Features.Authorization.Queries.Results;
using Shoofly.Data.Results.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Authorization.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManageUserClaimsResult>>
    {
        public string UserId { get; set; }

        public ManageUserClaimsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
