using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductBrandUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }
        public ProductBrandUpdatedEvent(Guid id, string brand, Guid modefiledBy)
        {
            Id = id;
            Brand = brand;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
