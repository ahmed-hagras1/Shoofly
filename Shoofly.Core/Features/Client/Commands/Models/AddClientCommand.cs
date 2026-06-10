using MediatR;
using Shoofly.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Features.Client.Commands.Models
{
    public class AddClientCommand : IRequest<Response<string>>
    {
        public string FullName { get; set; } = null!;

        // Combined field for Facebook-style login
        public string EmailOrPhone { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string PreferredLanguage { get; set; } = null!;
        public int CountryId { get; set; }
    }
}
