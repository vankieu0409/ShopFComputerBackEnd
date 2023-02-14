using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events
{
    class FunctionServiceNameChangedEvent : FunctionEventBase
    {
        public FunctionServiceNameChangedEvent(Guid id, string serviceName, Guid? modifiedBy):base(id)
        {
            ServiceName = serviceName;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }

        public string ServiceName { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTime ModifiedTime { get; private set; }
    }
}
