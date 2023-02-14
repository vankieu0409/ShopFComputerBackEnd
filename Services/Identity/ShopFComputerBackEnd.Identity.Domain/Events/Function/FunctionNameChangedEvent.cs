using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events
{
    class FunctionNameChangedEvent:FunctionEventBase
    {
        public FunctionNameChangedEvent(Guid id, string functionName, Guid? modifiedBy):base(id)
        {
            FunctionName = functionName;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }

        public string FunctionName { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTime ModifiedTime { get; private set; }
    }
}
