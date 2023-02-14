using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users
{
    public class ChangePasswordCommand : IRequest<UserDto>
    {
        public ChangePasswordCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
