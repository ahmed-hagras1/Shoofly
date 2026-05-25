using MediatR;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Bases;
using Shoofly.Core.Features.Authorization.Commands.Models;
using Shoofly.Service.Abstracts;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Authorization.Commands.Handlers
{
    public class AuthorizationCommandHandler : ResponseHandler,
        IRequestHandler<AddRoleCommand, Response<string>>,
        IRequestHandler<EditRoleCommand, Response<string>>,
        IRequestHandler<DeleteRoleCommand, Response<string>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public AuthorizationCommandHandler(IAuthorizationService authorizationService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _authorizationService = authorizationService;
            _localizer = localizer;
        }
        #endregion

        // CancellationToken is accepted here and passed down naturally by MediatR
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);

            if (result == "RoleIsExist")
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.RoleIsExist]);
            }
            else if (result == "Success")
            {
                return Success<string>(_localizer[SharedResourcesKeys.Created]);
            }

            return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
        }
        public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.EditRoleAsync(request.Id, request.Name);

            if (result == "NotFound")
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }
            else if (result == "Success")
            {
                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            }

            return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
        }
        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeleteRoleAsync(request.Id);

            if (result == "NotFound")
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }
            else if (result == "HasUsers")
            {
                // Return 400 BadRequest with our specific protective message
                return BadRequest<string>(_localizer[SharedResourcesKeys.RoleHasUsers]);
            }
            else if (result == "Success")
            {
                return Success<string>(_localizer[SharedResourcesKeys.Deleted]);
            }

            return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
        }
    }
}
