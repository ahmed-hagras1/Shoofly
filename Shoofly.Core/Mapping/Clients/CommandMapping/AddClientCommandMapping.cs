using AutoMapper;
using Shoofly.Core.Features.Client.Commands.Models;
using Shoofly.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.Mapping.Clients
{
    public partial class ClientProfile
    {
        // This is the private method called in the main constructor
        private void AddClientCommandMapping()
        {
            CreateMap<AddClientCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.EmailOrPhone))

                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                    src.EmailOrPhone.Contains("@") ? src.EmailOrPhone : null))

                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    !src.EmailOrPhone.Contains("@") ? src.EmailOrPhone : null))

                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }
    }
}
