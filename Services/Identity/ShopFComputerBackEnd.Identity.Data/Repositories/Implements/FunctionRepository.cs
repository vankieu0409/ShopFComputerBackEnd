using Iot.Core.Data.Relational.Repositories.Implements;
using Iot.Core.Extensions;

using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Data.Repositories.Implements
{
    public class FunctionRepository: Repository<FunctionReadModel>,IFunctionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FunctionRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        
    }
}
