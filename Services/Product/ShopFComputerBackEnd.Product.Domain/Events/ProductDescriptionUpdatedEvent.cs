using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductDescriptionUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }
        public ProductDescriptionUpdatedEvent(Guid id, string description, Guid modefiledBy)
        {
            Id = id;
            Description = description;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
