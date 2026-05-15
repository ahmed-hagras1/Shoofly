using MediatR;
using Shoofly.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Models
{
    public class RevokeTokenCommand : IRequest<Response<string>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
