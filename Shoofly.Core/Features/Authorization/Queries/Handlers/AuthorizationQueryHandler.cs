using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Bases;
using Shoofly.Core.Features.Authorization.Queries.Models;
using Shoofly.Core.Features.Authorization.Queries.Results;
using Shoofly.Service.Abstracts;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Authorization.Queries.Handlers
{
    public class AuthorizationQueryHandler : ResponseHandler,
        IRequestHandler<GetRoleListQuery, Response<List<GetRoleListResult>>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public AuthorizationQueryHandler(IAuthorizationService authorizationService,
            IStringLocalizer<SharedResources> localizer,
            IMapper mapper) : base(localizer)
        {
            _authorizationService = authorizationService;
            _localizer = localizer;
            _mapper = mapper;
        }
        #endregion
        #region Handlers
        public async Task<Response<List<GetRoleListResult>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            // Get roles from the service
            var roles = await _authorizationService.GetRolesListAsync(cancellationToken);

            // Map to the DTO result.
            var mappedRoles = _mapper.Map<List<GetRoleListResult>>(roles);

            // Return the standard Success response
            return Success(mappedRoles);
        }
        #endregion
    }
}
