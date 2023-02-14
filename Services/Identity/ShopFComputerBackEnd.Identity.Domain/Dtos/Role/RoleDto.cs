using Iot.Core.Domain.Implements;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class RoleDto : FullAuditedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
