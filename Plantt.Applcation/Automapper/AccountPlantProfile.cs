using AutoMapper;
using Plantt.Domain.DTOs.AccountPlant;
using Plantt.Domain.DTOs.AccountPlant.Request;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class AccountPlantProfile : Profile
    {
        public AccountPlantProfile()
        {
            CreateMap<AccountPlantEntity, AccountPlantDTO>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.PlantData));

            CreateMap<AccountPlantEntity, AccountPlantMinimumDTO>();

            CreateMap<UpdateAccountPlantRequest, AccountPlantEntity>();
        }
    }
}
