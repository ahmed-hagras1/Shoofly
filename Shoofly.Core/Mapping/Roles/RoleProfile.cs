using AutoMapper;
using Shoofly.Core.Features.Authorization.Queries.Results;
using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shoofly.Core.Mapping.Roles
{
    public partial class RoleProfile : Profile
    {
        public RoleProfile()
        {
            // Maps the properties automatically based on matching names (Id to Id, Name to Name)
            CreateMap<ApplicationRole, GetRoleListResult>();
            CreateMap<ApplicationRole, GetRoleByIdResult>();
        }
    }
}
