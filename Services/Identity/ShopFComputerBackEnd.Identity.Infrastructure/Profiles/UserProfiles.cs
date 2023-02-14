using System;
using System.Collections.Generic;
using ShopFComputerBackEnd.Identity.Domain.Aggregates;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using ShopFComputerBackEnd.Identity.Domain.Events.Users;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands;
using ShopFComputerBackEnd.Profile.Grpc.Protos;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Profiles
{
    public class UserProfiles : AutoMapper.Profile
    {
        public UserProfiles()
        {
            CreateMap<UserAggregateRoot , UserDto>().ReverseMap();
            CreateMap<ProfileDetailGrpcDto, UserProfileDto>().ReverseMap();
            CreateMap<AvatarGrpcDto, AvatarValueObject>().ReverseMap();
            CreateMap<UserAggregateRoot , RoleDto>().ReverseMap();
            CreateMap<UserAggregateRoot , AssignUserToRoleDto>().ReverseMap();
            CreateMap<ApplicationUserReadModel, UserDto>().ReverseMap();
            CreateMap<ApplicationUserReadModel, SignUpCommand>().ReverseMap();
            CreateMap<UserInitializedEvent, ApplicationUserReadModel>();
            CreateMap<OtpDto, ApplicationUserReadModel>().ReverseMap();
            CreateMap<ProfileDetailCollectionGrpcDto, List<Guid>>().ReverseMap();
        }
    }
}
