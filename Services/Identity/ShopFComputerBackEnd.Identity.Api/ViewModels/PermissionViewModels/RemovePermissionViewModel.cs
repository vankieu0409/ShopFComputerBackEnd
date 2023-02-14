using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class RemovePermissionViewModel
    {
        public Guid TypeId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
