using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Bases;
using Shoofly.Core.Features.Auth.Commands.Models;
using Shoofly.Data.Entities.Identity;
using Shoofly.Service.Abstracts;
using Shoofly.Shared.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Handlers
{
    public class VerifyCodeCommandHandler : ResponseHandler,
        IRequestHandler<VerifyCodeCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public VerifyCodeCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _userManager = userManager;
            _userService = userService;
            _localizer = localizer;
        }

        public async Task<Response<string>> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
        {
            // Find the user
            var user = await _userManager.FindByNameAsync(request.EmailOrPhone);

            if (user == null)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.UserNotFound]); // Add this key to ResX
            }

            // 2. Determine if it's an Email or Phone
            bool isEmail = request.EmailOrPhone.Contains("@");
            string method = isEmail ? "Email" : "Phone";

            // 3. Verify the code using your generic service!
            bool isValid = await _userService.VerifyCodeAsync(user, request.Code, method);

            if (!isValid)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidCode]); // Add this key to ResX
            }

            // 4. Success!
            return Success<string>(_localizer[SharedResourcesKeys.AccountVerifiedSuccessfully]); // Add this key to ResX
        }
    }
}