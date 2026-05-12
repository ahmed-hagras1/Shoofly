using FluentValidation;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Features.Client.Commands.Models;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Client.Commands.Validations
{
    public class AddClientValidator : AbstractValidator<AddClientCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructor
        public AddClientValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRules();
            ApplyCustomValidations();
        }
        #endregion
        #region Methods


        private void ApplyValidationRules()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100);

            RuleFor(x => x.Password)
                // CascadeMode.Stop means if it's empty, it won't run the Regex checks
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MinimumLength(6).WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordRequirements])
                .Matches("[a-z]").WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordRequirements])
                .Matches("[0-9]").WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordRequirements]);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordsDoNotMatch]);

            RuleFor(x => x.PreferredLanguage)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.EmailOrPhone)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                // Pass the localizer into the custom method if you need custom error logic, 
                // or just attach the message here:
                .Must(IsValidEmailOrPhone).WithMessage(_stringLocalizer[SharedResourcesKeys.InvalidEmailOrPhone]); 
        }
        private bool IsValidEmailOrPhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            bool isEmail = Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            bool isPhone = Regex.IsMatch(input, @"^\+?[0-9]{10,15}$");

            return isEmail || isPhone;
        }
        private void ApplyCustomValidations()
        {
            // Add any custom validation logic here if needed in the future.
        }
        #endregion
    }
}
