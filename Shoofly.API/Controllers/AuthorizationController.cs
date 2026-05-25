using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoofly.Api.Base;
using Shoofly.Core.AppMetaData;
using Shoofly.Core.Features.Authorization.Commands.Models;
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
        [HttpGet(Router.AuthorizationRouting.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] string id, CancellationToken cancellationToken)
        {
            // The [FromRoute] attribute grabs the {id} directly from the URL path
            var response = await Mediator.Send(new GetRoleByIdQuery(id),cancellationToken);
            return NewResult(response);
        }
        [HttpPost(Router.AuthorizationRouting.CreateRole)]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleCommand command, CancellationToken cancellationToken)
        {
            // The CancellationToken is passed to MediatR here
            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }
        [HttpPut(Router.AuthorizationRouting.EditRole)]
        public async Task<IActionResult> EditRole([FromBody] EditRoleCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }
        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] string id, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new DeleteRoleCommand(id), cancellationToken);
            return NewResult(response);
        }
    }
}
