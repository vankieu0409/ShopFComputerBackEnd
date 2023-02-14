using MediatR;
using System;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class IsUserExistQuery : IRequest<bool>
    {
        public IsUserExistQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
