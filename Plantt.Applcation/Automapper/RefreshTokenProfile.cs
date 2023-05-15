using AutoMapper;
using Plantt.Domain.DTOs.RefreshToken;
using Plantt.Domain.Entities;

namespace Plantt.Applcation.Automapper
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshTokenDTO, RefreshTokenEntity>();
            CreateMap<RefreshTokenEntity, RefreshTokenDTO>();
        }
    }
}
