using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Profile.Api.ViewModels;
using ShopFComputerBackEnd.Profile.Domain.Aggregates;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.ReadModels;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using ShopFComputerBackEnd.Profile.Grpc.Protos;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;
using System.Linq;

namespace ShopFComputerBackEnd.Profile.Api.Profiles
{
    public class MainProfile : AutoMapper.Profile
    {
        public MainProfile()
        {
            CreateMap<ProfileDto, CreateProfileViewModel>().ReverseMap();
            CreateMap<ProfileDto, UpdateProfileViewModel>().ReverseMap();
            CreateMap<CreateProfileViewModel, CreateProfileCommand>().ReverseMap();
            CreateMap<ProfileInitializedEvent, ProfileReadModel>().ReverseMap();
            CreateMap<CreateProfileCommand, ProfileDto>().ReverseMap();
            CreateMap<ProfileAggregateRoot, ProfileDto>().ReverseMap();
            CreateMap<UpdateProfileViewModel, UpdateProfileCommand>().ReverseMap();
            CreateMap<CreateProfileViewModel, UpdateProfileCommand>().ReverseMap();
            CreateMap<UserSignUpIntegrationEvent, CreateProfileCommand>().ReverseMap();

            CreateMap<ProfileDto, SelfCreateProfileViewModel>().ReverseMap();
            CreateMap<ProfileDto, SelfUpdateProfileViewModel>().ReverseMap();
            CreateMap<SelfCreateProfileViewModel, CreateProfileCommand>().ReverseMap();
            CreateMap<ProfileInitializedEvent, ProfileReadModel>().ReverseMap();
            CreateMap<CreateProfileCommand, ProfileDto>().ReverseMap();
            CreateMap<ProfileAggregateRoot, ProfileDto>().ReverseMap();
            CreateMap<SelfUpdateProfileViewModel, UpdateProfileCommand>().ReverseMap();
            CreateMap<UserSignUpIntegrationEvent, CreateProfileCommand>().ReverseMap();

            CreateMap<IQueryable<ProfileDetailGrpcDto>, IQueryable<ProfileDto>>().ReverseMap();

            // mapper Grpc Request
            CreateMap<CreateProfileGrpcCommand, CreateProfileCommand>();
            CreateMap<AvatarGrpcDto, AvatarValueObject>().ReverseMap();
            CreateMap<AddressGrpcDto, AddressValueObject>().ReverseMap();

            CreateMap<UpdateProfileGrpcCommand, UpdateProfileCommand>();

        }
    }
}
