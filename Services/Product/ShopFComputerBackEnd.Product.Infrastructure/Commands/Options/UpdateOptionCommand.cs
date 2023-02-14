using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using System;

namespace ShopFComputerBackEnd.Product.Infrastructure.Commands.Options
{
    public class UpdateOptionCommand : IRequest<OptionDto>
    {
        public UpdateOptionCommand(Guid id, string name, bool isEnabled)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            if (name.IsNullOrDefault())
                throw new ArgumentNullException(nameof(name));
            Name = name;
            IsEnabled = isEnabled; 
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
