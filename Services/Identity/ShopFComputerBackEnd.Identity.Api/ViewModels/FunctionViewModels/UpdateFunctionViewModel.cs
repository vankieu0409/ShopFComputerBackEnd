using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class UpdateFunctionViewModel
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
    }
}
