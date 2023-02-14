using Iot.Core.Data.Relational.Repositories.Implements;

using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using System;

namespace ShopFComputerBackEnd.Identity.Data.Repositories.Implements
{
    public class RoleRepository : Repository<ApplicationRoleReadModel>, IRoleRepository
    {

        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
