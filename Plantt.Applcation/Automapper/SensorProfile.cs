using AutoMapper;
using Plantt.Domain.DTOs.Sensor;
using Plantt.Domain.DTOs.Sensor.Request;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class SensorProfile : Profile
    {
        public SensorProfile()
        {
            CreateMap<SensorEntity, SensorDTO>()
                .ForMember(dest => dest.MyPlant, opt => opt.MapFrom(src => src.AccountPlant));

            CreateMap<UpdateSensorRequest, SensorEntity>()
                .ForMember(dest => dest.AccountPlantId, opt => opt.MapFrom(src => src.MyPlantId));
        }
    }
}
