using AutoMapper;
using Plantt.Domain.DTOs.PlantData;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class PlantDataProfile : Profile
    {
        public PlantDataProfile()
        {
            CreateMap<PlantDataEntity, PlantDataDTO>()
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.CreatedTS));
        }
    }
}
