using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GenerateSignInTokenQuery : IRequest<UserDto>
    {

        public ApplicationUserReadModel User { get; set; }
        public Guid ActiveProfileId { get; set; }

        public GenerateSignInTokenQuery(ApplicationUserReadModel user, Guid activeProfileId)
        {
            User = user;
            ActiveProfileId = activeProfileId;
        }
    }
}
