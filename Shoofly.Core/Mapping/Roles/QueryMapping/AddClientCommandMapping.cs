using AutoMapper;
using Shoofly.Core.Features.Authorization.Queries.Results;
using Shoofly.Core.Features.Client.Commands.Models;
using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        // This is the private method called in the main constructor
        private void AddRoleQueryMapping()
        {
            // Maps the properties automatically based on matching names (Id to Id, Name to Name)
            CreateMap<ApplicationRole, GetRoleListResult>();
        }
    }
}
