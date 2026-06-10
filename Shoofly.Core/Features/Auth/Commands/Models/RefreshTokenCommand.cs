using MediatR;
using Shoofly.Core.Bases;
using Shoofly.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JWTAuthResult>>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
