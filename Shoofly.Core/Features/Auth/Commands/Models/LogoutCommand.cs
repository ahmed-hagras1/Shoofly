using MediatR;
using Shoofly.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Models
{
    public class LogoutCommand : IRequest<Response<string>>
    {
        public string AccessToken { get; set; }
    }
}
