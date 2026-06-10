using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoofly.Api.Base;
using Shoofly.Core.AppMetaData;
using Shoofly.Core.Features.Client.Commands.Models;

namespace Shoofly.API.Controllers
{
    [ApiController]
    public class ClientController : AppControllerBase
    {
        [HttpPost(Router.ClientRouting.Register)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient([FromBody] AddClientCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }
    }
}
