using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class PermissionDto
    {
        public PermissionType Type  { get; set; }
        public Guid TypeId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
