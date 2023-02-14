using ShopFComputerBackEnd.Identity.Domain.Aggregates;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.Events.Roles;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;
using Microsoft.AspNetCore.Identity;
using ShopFComputerBackEnd.Identity.Domain.Dtos.Role;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Profiles
{
    class RoleProfile:AutoMapper.Profile
    {
        public RoleProfile()
        {
            CreateMap<ApplicationRoleReadModel, RoleDto>().ReverseMap();
            CreateMap<RoleAggregateRoot, RoleDto>().ReverseMap();
            CreateMap<RoleInitializedEvent, ApplicationRoleReadModel>().ReverseMap();
            CreateMap<AssignUserToRoleDto, RoleDto>().ReverseMap();
            CreateMap<AssignUserToRoleDto, IdentityResult>().ReverseMap();
            CreateMap<AssignRoleToUserDto, AssignRoleToUserCommand>().ReverseMap();
            CreateMap<AssignUserToRolesDto, AssignUserToRolesCommand>().ReverseMap();
            CreateMap<RoleDto, IdentityResult>().ReverseMap();
        }
    }
}
