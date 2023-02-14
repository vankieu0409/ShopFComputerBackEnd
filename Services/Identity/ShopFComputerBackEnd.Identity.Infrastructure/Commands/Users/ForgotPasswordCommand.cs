using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users
{
    public class ForgotPasswordCommand : IRequest<UserDto>
    {
        public ForgotPasswordCommand(Guid id, string phoneNumber, string newPassword, string confirmPassword)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
