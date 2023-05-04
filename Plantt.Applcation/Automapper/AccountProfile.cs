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
            CreateMap<AccountDto, AccountEntity>();
            CreateMap<AccountEntity, AccountDto>();

            CreateMap<AccountEntity, Password>();
        }
    }
}
