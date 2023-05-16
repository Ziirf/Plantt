using AutoMapper;
using Plantt.Domain.DTOs.Room;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {

            CreateMap<RoomEntity, RoomDTO>();
        }
    }
}
