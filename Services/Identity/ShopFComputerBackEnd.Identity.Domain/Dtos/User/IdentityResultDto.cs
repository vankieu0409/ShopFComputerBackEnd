using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class IdentityResultDto : IdentityResult
    {
        public object UserDto { get; set; }
        public object RoleDto { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
