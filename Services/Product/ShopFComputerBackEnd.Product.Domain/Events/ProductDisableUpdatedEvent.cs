using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductDisableUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }

        public ProductDisableUpdatedEvent(Guid id, bool isdeleted, Guid modefiledBy)
        {
            Id = id;
            IsDeleted = isdeleted;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
