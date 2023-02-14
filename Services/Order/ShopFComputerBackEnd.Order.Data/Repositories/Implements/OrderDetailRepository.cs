using Iot.Core.Data.Relational.Repositories.Implements;
using ShopFComputerBackEnd.Order.Data;
using ShopFComputerBackEnd.Order.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Order.Domain.ReadModels;
using System;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Data.Repositories.Implements
{
    public class OrderDetailRepository : Repository<OrderDetailEntity>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context, context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
