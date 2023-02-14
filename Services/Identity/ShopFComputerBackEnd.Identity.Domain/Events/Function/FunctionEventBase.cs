using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events
{
    public class FunctionEventBase
    {
        internal FunctionEventBase(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }
}
