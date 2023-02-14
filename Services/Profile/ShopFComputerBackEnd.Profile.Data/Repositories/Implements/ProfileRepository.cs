using Iot.Core.Data.Relational.Repositories.Implements;

using ShopFComputerBackEnd.Profile.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Profile.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Data.Repositories.Implements
{
    public class ProfileRepository : Repository<ProfileReadModel>, IProfileRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProfileRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
