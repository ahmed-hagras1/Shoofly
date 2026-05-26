using FluentValidation;
using Microsoft.Extensions.Localization;
using Shoofly.Core.Features.Authorization.Queries.Models;
using Shoofly.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Authorization.Queries.Validations
{
    public class ManageUserRolesValidator : AbstractValidator<ManageUserRolesQuery>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ManageUserRolesValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaximumLength]);
        }
    }
}
