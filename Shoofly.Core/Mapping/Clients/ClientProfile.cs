using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shoofly.Core.Mapping.Clients
{
    public partial class ClientProfile : Profile
    {
        public ClientProfile()
        {
            // Client Mapping.
            AddClientCommandMapping();


        }
    }
}
