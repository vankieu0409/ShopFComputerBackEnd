using ShopFComputerBackEnd.Identity.Domain.Aggregates;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Profiles
{
    public class RefreshTokenProfile : AutoMapper.Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshTokenReadModel, RefreshTokenDto>().ForMember(dest => dest.RefreshTokenExpireTime, opt => opt.MapFrom(src => src.ExpiredTime)).ReverseMap();
            CreateMap<RefreshTokenDto, CreateRefreshTokenCommand>().ReverseMap();
            CreateMap<CreateRefreshTokenCommand, RefreshTokenReadModel>().ReverseMap();
            CreateMap<UserAggregateRoot, RefreshTokenDto>().ReverseMap();
            CreateMap<RevokeRefreshTokenCommand, RefreshTokenReadModel>().ReverseMap();
        }
    }
}
