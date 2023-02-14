using Iot.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Domain.ReadModels
{
    public class PermissionReadModel : Entity
    {
        public PermissionReadModel(PermissionType type, Guid typeId, Guid functionId)
        {
            Type = type;
            TypeId = typeId;
            FunctionId = functionId;
        }
        public PermissionType Type { get; set; }
        public Guid TypeId { get; set; }
        [ForeignKey("Function")]
        public Guid FunctionId { get; set; }
        public FunctionReadModel Function { get; set; }
    }
}
