using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductCategoryUpdatedEvent :EventBase
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }
        public ProductCategoryUpdatedEvent(Guid id, string category, Guid modefiledBy)
        {
            Id = id;
            Category = category;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
