using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost(Router.AuthRouting.ResendCode)]
        public async Task<IActionResult> ResendCode([FromBody] ResendCodeCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }
        [HttpPost(Router.AuthRouting.SignIn)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            return NewResult(response);
        }
        [HttpPost(Router.AuthRouting.Logout)]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Extract the Access Token from the "Authorization: Bearer <token>" header
            // This is built into ASP.NET Core Authentication
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Fallback: If for some reason GetTokenAsync fails, extract it manually
            if (string.IsNullOrEmpty(accessToken))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                accessToken = authHeader?.Replace("Bearer ", "").Trim();
            }

            // Create the Command and send it to the Handler via Mediator
            var command = new LogoutCommand { AccessToken = accessToken! };
            var response = await Mediator.Send(command);

            // Return the standardized response
            return NewResult(response);
        }

        [HttpPost(Router.AuthRouting.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpPost(Router.AuthRouting.RevokeToken)]
        [Authorize] // Only logged-in users (or admins) can revoke tokens
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpPost(Router.AuthRouting.RevokeAllSessions)]
        [Authorize] 
        public async Task<IActionResult> RevokeAllSessions([FromBody] RevokeAllSessionsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
