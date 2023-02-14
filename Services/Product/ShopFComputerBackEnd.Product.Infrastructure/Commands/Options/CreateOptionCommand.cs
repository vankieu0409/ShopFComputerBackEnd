using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using System;

namespace ShopFComputerBackEnd.Product.Infrastructure.Commands.Options
{
    public class CreateOptionCommand : IRequest<OptionDto>
    {
        public CreateOptionCommand(Guid id, string name)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            if (name.IsNullOrDefault())
                throw new ArgumentNullException(nameof(name));
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
