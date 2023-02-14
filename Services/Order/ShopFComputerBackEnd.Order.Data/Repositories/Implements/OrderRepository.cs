using Iot.Core.Data.Relational.Repositories.Implements;
using ShopFComputerBackEnd.Order.Data;
using ShopFComputerBackEnd.Order.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Order.Domain.ReadModels;
using System;

namespace ShopFComputerBackEnd.Order.Data.Repositories.Implements
{
    public class OrderRepository : Repository<OrderReadModel>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context, context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
