using AutoMapper;
using Plantt.Domain.DTOs.Plant;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    internal class PlantProfile : Profile
    {
        public PlantProfile()
        {
            CreateMap<PlantEntity, PlantDTO>()
                .ForMember(dest => dest.Watering, opt => opt.MapFrom(src => src.PlantWatering!.Text));

            CreateMap<PlantEntity, PlantMinimumDTO>();
        }
    }
}
