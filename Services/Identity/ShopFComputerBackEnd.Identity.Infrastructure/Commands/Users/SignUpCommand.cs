using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands
{
    public class SignUpCommand : IRequest<UserDto>
    {
        public SignUpCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordSalt { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
