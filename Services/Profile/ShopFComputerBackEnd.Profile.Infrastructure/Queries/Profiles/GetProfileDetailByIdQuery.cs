using ShopFComputerBackEnd.Profile.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles
{
    public class GetProfileDetailByIdQuery : IRequest<ProfileDto>
    {
        public GetProfileDetailByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
