using FluentValidation;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Features.Auth.Commands.Models;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Validations
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RefreshTokenValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }
        private void ApplyValidationRules()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }
    }
}
