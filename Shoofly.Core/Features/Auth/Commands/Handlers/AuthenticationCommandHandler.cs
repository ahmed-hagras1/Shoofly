using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Shoofly.Core.Bases;
using Shoofly.Core.Features.Auth.Commands.Models;
using Shoofly.Data.Entities.Identity;
using Shoofly.Data.Helpers;
using Shoofly.Infrastructure.Data;
using Shoofly.Service.Abstracts;
using Shoofly.Service.Implementations;
using Shoofly.Shared.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
        IRequestHandler<VerifyCodeCommand, Response<string>>,
        IRequestHandler<SignInCommand, Response<JWTAuthResult>>,
        IRequestHandler<LogoutCommand, Response<string>>,
        IRequestHandler<ResendCodeCommand, Response<string>>,
        IRequestHandler<RefreshTokenCommand, Response<JWTAuthResult>>,
        IRequestHandler<RevokeTokenCommand, Response<string>>,
        IRequestHandler<RevokeAllSessionsCommand, Response<string>>,
        IRequestHandler<ForgotPasswordCommand, Response<string>>,
        IRequestHandler<VerifyResetCodeCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly AppDbContext _dbContext;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AuthenticationCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserService userService, 
            IStringLocalizer<SharedResources> localizer,
            IAuthenticationService authenticationService,
            IEmailService emailService, 
            ISmsService smsService,
            AppDbContext appDbContext) : base(localizer)
        {
            _userManager = userManager;
            _userService = userService;
            _localizer = localizer;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _smsService = smsService;
            _dbContext = appDbContext;
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

        public async Task<Response<JWTAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // 1. Find User by Email or Phone Number
            bool isEmailLogin = request.LoginIdentifier.Contains("@");
            var user = isEmailLogin
                ? await _userManager.FindByEmailAsync(request.LoginIdentifier)
                : await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.LoginIdentifier, cancellationToken);

            // 2. Check if user exists
            if (user == null)
            {
                return BadRequest<JWTAuthResult>(_localizer[SharedResourcesKeys.EmailOrPasswordIsWrong]);
            }

            // 3. Check if Password is correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest<JWTAuthResult>(_localizer[SharedResourcesKeys.EmailOrPasswordIsWrong]);
            }

            // 4. Check Business Rules: Account Status
            if (!user.IsActive)
            {
                return BadRequest<JWTAuthResult>(_localizer[SharedResourcesKeys.AccountIsDisabled]);
            }

            // 5. Check if Email or Phone is confirmed (New Validation)
            if (isEmailLogin)
            {
                if (!user.EmailConfirmed)
                {
                    return BadRequest<JWTAuthResult>(_localizer[SharedResourcesKeys.EmailNotConfirmed]);
                }
            }
            else
            {
                if (!user.PhoneNumberConfirmed)
                {
                    return BadRequest<JWTAuthResult>(_localizer[SharedResourcesKeys.PhoneNotConfirmed]);
                }
            }

            // 6. Generate Tokens
            var result = await _authenticationService.GetJWTToken(user);
            return Success(result);
        }

        public async Task<Response<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // Logic: Call the service to revoke the token in the database.
            // The service might throw a SecurityTokenException if the token is invalid.
            var resultKey = await _authenticationService.RevokeRefreshToken(request.AccessToken);

            // Return: If we reach here, it means the operation was successful.
            // resultKey is typically "LoggedOutSuccessfully"
            return Success<string>(_localizer[resultKey]);
        }

        public async Task<Response<string>> Handle(ResendCodeCommand request, CancellationToken cancellationToken)
        {
            // 1. Find the user
            var user = await _userManager.FindByNameAsync(request.EmailOrPhone);
            if (user == null)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.UserNotFound]);
            }

            // 2. Check if already verified (Optional but recommended)
            bool isEmail = request.EmailOrPhone.Contains("@");
            if ((isEmail && user.EmailConfirmed) || (!isEmail && user.PhoneNumberConfirmed))
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.AccountAlreadyVerified]);
            }

            // 3. Generate New Code
            string method = isEmail ? "Email" : "Phone";
            string verificationCode = await _userService.GenerateVerificationCodeAsync(user, method);

            // 4. Send the Code
            if (isEmail)
            {
                string subject = "Shoofly: Your New Verification Code";
                string body = $"<p>Your new verification code is: <strong>{verificationCode}</strong></p>";
                await _emailService.SendEmailAsync(user.Email!, subject, body, cancellationToken);
            }
            else
            {
                // SMS Logic (Reusing your formatting logic)
                string formattedPhone = user.PhoneNumber!.TrimStart('0');
                var country = await _dbContext.Countries.FindAsync(user.CountryId, cancellationToken);
                string dialCode = country?.DialCode ?? "+20";
                string fullPhoneNumber = string.Concat(dialCode, formattedPhone);

                string smsMessage = $"Your new Shoofly code is: {verificationCode}";
                await _smsService.SendSmsAsync(fullPhoneNumber, smsMessage, cancellationToken);
            }

            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<JWTAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.GetRefreshToken(request.AccessToken, request.RefreshToken);

            return Success(result);
        }

        public async Task<Response<string>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            // Call the new service method
            var resultKey = await _authenticationService.RevokeToken(request.RefreshToken);

            // Return the localized success message
            return Success<string>(_localizer[resultKey]);
        }

        public async Task<Response<string>> Handle(RevokeAllSessionsCommand request, CancellationToken cancellationToken)
        {
            var resultKey = await _authenticationService.RevokeAllSessions(request.UserId);

            return Success<string>(_localizer[resultKey]);
        }

        public async Task<Response<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var messageKey = await _authenticationService.ForgotPasswordAsync(request.EmailOrPhone, cancellationToken);

            if (messageKey == SharedResourcesKeys.UserNotFound)
            {
                return BadRequest<string>(_localizer[messageKey].Value);
            }

            return Success(_localizer[messageKey].Value);
        }

        public async Task<Response<string>> Handle(VerifyResetCodeCommand request, CancellationToken cancellationToken)
        {
            var messageKey = await _authenticationService.VerifyResetCodeAsync(request.EmailOrPhone, request.Code, cancellationToken);

            if (messageKey == SharedResourcesKeys.InvalidOrExpiredCode)
            {
                return BadRequest<string>(_localizer[messageKey].Value);
            }

            return Success(_localizer[messageKey].Value);
        }
    }
}