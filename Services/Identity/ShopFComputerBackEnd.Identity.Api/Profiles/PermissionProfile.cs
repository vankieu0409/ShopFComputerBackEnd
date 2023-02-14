using AutoMapper;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;

namespace ShopFComputerBackEnd.Identity.Api.Profiles
{
    public class PermissionProfile : AutoMapper.Profile
    {
        public PermissionProfile()
        {
            CreateMap<CreatePermissionViewModel, AssignPermissionCommand>().ReverseMap();
        }
    }
}
