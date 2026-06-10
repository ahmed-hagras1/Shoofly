using MediatR;
using Shoofly.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Models
{
    public class VerifyResetCodeCommand : IRequest<Response<string>>
    {
        public string EmailOrPhone { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
