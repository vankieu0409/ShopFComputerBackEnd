using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using System;

namespace ShopFComputerBackEnd.Product.Infrastructure.Queries.Options
{
    public class GetOptionByIdQuery : IRequest<OptionDto>
    {
        public GetOptionByIdQuery(Guid id)
        {
            if(id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
