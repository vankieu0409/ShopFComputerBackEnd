using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using System;

namespace ShopFComputerBackEnd.Product.Infrastructure.Commands.Options
{
    public class DisableOptionCammand:IRequest<OptionDto>
    {
        public DisableOptionCammand(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
