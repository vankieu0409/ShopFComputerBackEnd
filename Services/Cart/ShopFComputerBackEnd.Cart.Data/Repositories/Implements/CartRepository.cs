using Iot.Core.Data.Relational.Repositories.Implements;

using ShopFComputerBackEnd.Cart.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;

using System;

namespace ShopFComputerBackEnd.Cart.Data.Repositories.Implements
{
    public class CartRepository : Repository<CartReadModel>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context, context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
