using AutoMapper;
using Plantt.Domain.DTOs.Room;
using Plantt.Domain.DTOs.Room.Request;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {

            CreateMap<RoomEntity, RoomDTO>()
                .ForMember(dest => dest.MyPlants, opt => opt.MapFrom(src => src.Plants));

            CreateMap<UpdateRoomRequest, RoomEntity>();
        }
    }
}
