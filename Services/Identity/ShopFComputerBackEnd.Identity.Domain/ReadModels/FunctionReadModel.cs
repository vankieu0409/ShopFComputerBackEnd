using Iot.Core.Domain.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.ReadModels
{
    public class FunctionReadModel:FullEntity<Guid>
    {
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
        public ICollection<PermissionReadModel> Permissions { get; set; }
    }
}
