using AutoMapper;
using Plantt.Domain.DTOs.Account;
using Plantt.Domain.DTOs.Hub;
using Plantt.Domain.DTOs.Hub.Response;
using Plantt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Applcation.Automapper
{
    public class HubProfile : Profile
    {
        public HubProfile()
        {
            CreateMap<HubEntity, CreateHubResponse>();

            CreateMap<HubEntity, HubWithSecretDTO>()
                .ForMember(dest => dest.HomeName, opt => opt.MapFrom(src => src.Home.Name));

            CreateMap<HubEntity, HubDTO>()
                .ForMember(dest => dest.HomeName, opt => opt.MapFrom(src => src.Home.Name));
        }
    }
}
