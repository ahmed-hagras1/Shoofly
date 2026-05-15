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
    public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenValidator(IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(localizer[SharedResourcesKeys.NotEmpty]);
        }
    }
}
