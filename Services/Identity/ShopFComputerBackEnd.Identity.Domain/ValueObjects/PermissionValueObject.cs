using Iot.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Domain.ValueObjects
{
    public class PermissionValueObject:Entity
    {
        public PermissionType Type { get; set; }
        public Guid TypeId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
