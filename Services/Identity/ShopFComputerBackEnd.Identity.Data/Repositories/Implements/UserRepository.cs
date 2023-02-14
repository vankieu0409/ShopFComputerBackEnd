using Iot.Core.Data.Relational.Repositories.Implements;

using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using System;

namespace ShopFComputerBackEnd.Identity.Data.Repositories.Implements
{
    public class UserRepository : Repository<ApplicationUserReadModel>,IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

    }
}
