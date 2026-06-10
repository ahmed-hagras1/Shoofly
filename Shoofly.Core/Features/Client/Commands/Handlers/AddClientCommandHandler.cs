using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Bases;
using Shoofly.Core.Features.Client.Commands.Models;
using Shoofly.Data.Entities.Identity;
using Shoofly.Infrastructure.Data;
using Shoofly.Service.Abstracts;
using Shoofly.Service.Implementations;
using Shoofly.Shared.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Client.Commands.Handlers
{
    public class AddClientCommandHandler : ResponseHandler,
        IRequestHandler<AddClientCommand, Response<string>>
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly AppDbContext _dbContext;
        #endregion
        #region Constructor
        public AddClientCommandHandler(
            IUserService userService, 
            IStringLocalizer<SharedResources> localizer,
            IMapper mapper,
            IEmailService emailService,
            ISmsService smsService,
            AppDbContext dbContext) : base(localizer)
        {
            _userService = userService;
            _localizer = localizer;
            _mapper = mapper;
            _emailService = emailService;
            _smsService = smsService;
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists
            bool userExists = await _userService.IsEmailOrPhoneRegisteredAsync(request.EmailOrPhone);
            if (userExists)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.UserAlreadyExists]);
            }

            // Setup the Entity
            var identityUser = _mapper.Map<ApplicationUser>(request);

            // Register using the Service
            var errorMessage = await _userService.RegisterUserAsync(identityUser, request.Password, "Client");

            if (errorMessage != null)
            {
                return BadRequest<string>(errorMessage);
            }

            // GENERATE VERIFICATION CODE (NEW CODE HERE)
            bool isEmail = request.EmailOrPhone.Contains("@");
            string method = isEmail ? "Email" : "Phone";

            string verificationCode = await _userService.GenerateVerificationCodeAsync(identityUser, method);

            // This forces the server to wait for 10 seconds. 
            // If you cancel during this time, it throws an error and stops!
            //await Task.Delay(10000, cancellationToken);

            if (isEmail)
            {
                string subject = "Welcome to Shoofly! Your Verification Code";
                string body = $"<h3>Hello {identityUser.FullName},</h3><p>Your verification code is: <strong>{verificationCode}</strong></p>";

                await _emailService.SendEmailAsync(identityUser.Email, subject, body, cancellationToken);
            }
            else
            {
                string smsMessage = $"Welcome to Shoofly! Your verification code is: {verificationCode}";

                // 1. احذف الصفر الأول من رقم الهاتف
                string formattedPhone = identityUser.PhoneNumber.TrimStart('0');

                // 🟢 2. ابحث عن كود الدولة من قاعدة البيانات باستخدام الـ CountryId
                var country = await _dbContext.Countries.FindAsync(identityUser.CountryId, cancellationToken);

                // تأكد أنك وجدت الدولة، وإذا لم تجدها ضع (+20) كقيمة افتراضية احتياطية للأمان
                string dialCode = country != null ? country.DialCode : "+20";

                // 3. ادمج كود الدولة مع رقم الهاتف
                string fullPhoneNumber = string.Concat(dialCode, formattedPhone);

                // 4. أرسل الرسالة!
                await _smsService.SendSmsAsync(fullPhoneNumber, smsMessage, cancellationToken);
            }

            return Success<string>($"{_localizer[SharedResourcesKeys.Success]}");
        }
        #endregion
    }
}