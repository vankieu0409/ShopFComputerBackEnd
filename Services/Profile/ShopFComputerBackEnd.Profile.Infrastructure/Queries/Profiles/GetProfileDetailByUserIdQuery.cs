using ShopFComputerBackEnd.Profile.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles
{
    public class GetProfileDetailByUserIdQuery : IRequest<ProfileDto>
    {
        public GetProfileDetailByUserIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
