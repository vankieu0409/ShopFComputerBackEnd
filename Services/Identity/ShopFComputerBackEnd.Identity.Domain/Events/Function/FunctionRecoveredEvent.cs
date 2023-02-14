using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Function
{
    class FunctionRecoveredEvent : FunctionEventBase
    {
        public FunctionRecoveredEvent(Guid id, Guid? modifiedBy) : base(id)
        {
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid? ModifiedBy { get; private set; }
        public DateTime ModifiedTime { get; private set; }
    }
}
