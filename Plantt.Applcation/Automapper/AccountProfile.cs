using AutoMapper;
using Plantt.Domain.DTOs.Account;
using Plantt.Domain.Entities;
using Plantt.Domain.Models;

namespace Plantt.Applcation.Automapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDTO, AccountEntity>();
            CreateMap<AccountEntity, AccountDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString())); ;

            CreateMap<AccountEntity, Password>();
        }
    }
}
