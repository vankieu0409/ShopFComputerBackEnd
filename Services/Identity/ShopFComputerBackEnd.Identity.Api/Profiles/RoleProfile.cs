using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;

namespace ShopFComputerBackEnd.Identity.Api.Profiles
{
    public class RoleProfile:AutoMapper.Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleViewModel, CreateRoleCommand>().ReverseMap();
            CreateMap<UpdateRoleViewModel, UpdateRoleCommand>().ReverseMap();
            CreateMap<AssignRoleToUsersViewModel, AssignRoleToUserCommand>().ReverseMap();
        }
    }
}
