using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.shared.ValueObjectShared;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users;

namespace ShopFComputerBackEnd.Identity.Api.Profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<SignUpViewModel, SignUpCommand>();
            CreateMap<ForgotPasswordViewModel, ConfirmOtpCommand>();
            CreateMap<UpdateFunctionViewModel, UpdateFunctionCommand>().ReverseMap();
            CreateMap<ChangePasswordViewModel, ChangePasswordCommand>();
            CreateMap<SelfChangePasswordViewModel, ChangePasswordCommand>();
            CreateMap<AssignUserToRolesViewModel, AssignUserToRolesCommand>().ReverseMap();
            CreateMap<SignUpViewModel, AssignUserToRoleCommand>().ReverseMap();
            CreateMap<ConfirmOtpCommand, ConfirmOtpViewModel>().ReverseMap();
            CreateMap<CreateRefreshTokenCommand, RefreshTokenDto>().ReverseMap();
            CreateMap<DeviceValueObject, DeviceValueObjectShared>();
        }
    }
}
