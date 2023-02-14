using System;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class CreatePermissionViewModel
    {
        public PermissionType Type { get; set; }
        public Guid TypeId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
