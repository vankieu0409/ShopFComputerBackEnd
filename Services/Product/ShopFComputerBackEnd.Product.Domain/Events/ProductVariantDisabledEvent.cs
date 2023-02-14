using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductVariantDisabledEvent : EventBase
    {
        public Guid Id { get; set; }
        public bool IsDisabled { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }

        public ProductVariantDisabledEvent(Guid id, bool isDisabled, Guid modefiledBy)
        {
            Id = id;
            IsDisabled = isDisabled;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.Now;
        }
    }
}
