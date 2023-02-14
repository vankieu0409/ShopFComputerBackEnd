using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users
{
    public class ChangeOtpCommand : IRequest<UserDto>
    {
        public ChangeOtpCommand(Guid id, string otp)
        {
            Id = id;
            Otp = otp;
        }
        public Guid Id { get; set; }
        public string Otp { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
