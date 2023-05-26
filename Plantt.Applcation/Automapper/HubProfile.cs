using AutoMapper;
using Plantt.Domain.DTOs.Hub;
using Plantt.Domain.DTOs.Hub.Response;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class HubProfile : Profile
    {
        public HubProfile()
        {
            CreateMap<HubEntity, CreateHubResponse>();

            CreateMap<HubEntity, HubDTO>()
                .ForMember(dest => dest.HomeName, opt => opt.MapFrom(src => src.Home!.Name));
        }
    }
}
