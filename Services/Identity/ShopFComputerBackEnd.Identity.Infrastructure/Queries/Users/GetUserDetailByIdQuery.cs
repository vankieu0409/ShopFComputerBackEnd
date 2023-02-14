using MediatR;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using System;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GetUserDetailByIdQuery : IRequest<UserDto>
    {
        public GetUserDetailByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
