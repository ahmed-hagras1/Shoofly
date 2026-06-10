using FluentValidation;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Features.Authorization.Commands.Models;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Authorization.Commands.Validations
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddRoleValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaximumLength]);
        }
    }
}
