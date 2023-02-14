using Iot.Core.Data.Relational.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces
{
    public interface IPermissionRepository : IRepository<PermissionReadModel>
    {
    }
}
