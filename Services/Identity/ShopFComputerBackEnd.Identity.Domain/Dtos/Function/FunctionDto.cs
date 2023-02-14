using Iot.Core.Domain.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class FunctionDto: FullAuditedEntity
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
    }
}
