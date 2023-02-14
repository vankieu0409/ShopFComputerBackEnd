using Iot.Core.Data.Relational.Repositories.Implements;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Options;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using System;

namespace ShopFComputerBackEnd.Product.Data.Repositories.Implements.Options
{
    public class OptiontRepository : Repository<OptionReadModel>, IOptiontRepository
    {
        private readonly ApplicationDbContext _context;
        public OptiontRepository(ApplicationDbContext context) : base(context, context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
