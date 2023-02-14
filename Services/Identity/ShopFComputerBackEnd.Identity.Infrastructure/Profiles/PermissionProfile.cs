using AutoMapper;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Profiles
{
    class PermissionProfile: AutoMapper.Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionReadModel, PermissionDto>().ReverseMap();
            CreateMap<PermissionReadModel, AssignPermissionCommand>().ReverseMap();
        }
    }
}
