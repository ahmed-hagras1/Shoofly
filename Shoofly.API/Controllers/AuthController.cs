using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoofly.Api.Base;
using Shoofly.Core.AppMetaData;
using Shoofly.Core.Features.Auth.Commands.Models;

namespace Shoofly.API.Controllers
{
    [ApiController]
    public class AuthController : AppControllerBase
    {
        [HttpPost(Router.AuthRouting.VerifyCode)]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command,cancellationToken);
            return NewResult(response);
        }
    }
}
