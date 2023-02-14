using MediatR;
using System;
using System.Collections.Generic;
using ShopFComputerBackEnd.Identity.Domain.Dtos.Role;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles
{
    public class AssignRoleToUserCommand : IRequest<AssignRoleToUserDto>
    {
        public AssignRoleToUserCommand( Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
        public List<UserDto> UserDtos { get; set; }
    }
}
