using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Profile.Domain.Aggregates;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.ReadModels;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Profiles
{
    class MainProfile : AutoMapper.Profile
    {
        public MainProfile()
        {
            CreateMap<ProfileReadModel, ProfileDto>();
            CreateMap<ProfileDto, CreateProfileCommand>().ReverseMap();
            CreateMap<ProfileInitializedEvent, ProfileReadModel>().ReverseMap();
            CreateMap<ProfileAggregateRoot, ProfileReadModel>().ReverseMap();
            CreateMap<ProfileAggregateRoot, ProfileDto>().ReverseMap();
            CreateMap<UserSignUpIntegrationEvent, UpdateProfileCommand>().ReverseMap();
            CreateMap<ProfileReadModel, UpdateProfileCommand>().ReverseMap();
        }
    }
}
