using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Enums;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users
{
    public class ConfirmOtpCommand : IRequest<UserDto>
    {
        public ConfirmOtpCommand(Guid id, string user)
        {
            Id = id;
            User = user;
        }
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Otp { get; set; }
        public OtpType OtpType { get; set; }
    }
}
