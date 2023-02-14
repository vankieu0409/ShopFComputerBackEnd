using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Function
{
    class FunctionDeletedEvent : FunctionEventBase
    {
        public FunctionDeletedEvent(Guid id, Guid? deleteBy):base(id)
        {
            IsDeleted = true;
            DeletedBy = deleteBy;
            DeletedTime = DateTime.UtcNow;
        }

        public bool IsDeleted { get;private set; }
        public Guid? DeletedBy { get; private set; }
        public DateTime DeletedTime { get; private set; }
    }
}
