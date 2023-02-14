using MediatR;
using System;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles
{
    public class IsProfileExistQuery : IRequest<bool>
    {
        public IsProfileExistQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
