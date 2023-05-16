using AutoMapper;
using Plantt.Domain.DTOs.Home;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            CreateMap<HomeEntity, HomeDTO>();
        }
    }
}
