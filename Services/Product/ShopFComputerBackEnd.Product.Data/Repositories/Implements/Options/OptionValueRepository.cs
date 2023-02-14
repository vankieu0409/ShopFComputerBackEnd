using Iot.Core.Data.Relational.Repositories.Implements;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Options;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Data.Repositories.Implements.Options
{
    public class OptionValueRepository : Repository<OptionValueReadModel>, IOptionValueRepository
    {
        private readonly ApplicationDbContext _context;
        public OptionValueRepository(ApplicationDbContext context) : base(context, context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
