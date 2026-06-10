using MediatR;
using Shoofly.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Auth.Commands.Models
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {
        // Ignored in Swagger/JSON body. We will populate this in the Controller.
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
