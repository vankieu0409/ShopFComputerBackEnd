using ShopFComputerBackEnd.Profile.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles
{
    public class DeleteProfileCommand : IRequest<ProfileDto>
    {
        public DeleteProfileCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
