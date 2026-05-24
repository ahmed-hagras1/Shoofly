using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoofly.Api.Base;
using Shoofly.Core.AppMetaData;
using Shoofly.Core.Features.Authorization.Queries.Models;

namespace Shoofly.API.Controllers
{
    [ApiController]
    // Protects ALL endpoints in this controller so only users with the "Admin" role can access them
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : AppControllerBase
    {
        [HttpGet(Router.AuthorizationRouting.GetRolesList)]
        public async Task<IActionResult> GetRoleList(CancellationToken cancellationToken)
        {
            // Send the empty query to MediatR
            var response = await Mediator.Send(new GetRoleListQuery(), cancellationToken);

            // Return the formatted result
            return NewResult(response);
        }
    }
}
