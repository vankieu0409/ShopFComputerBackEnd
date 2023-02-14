using Iot.Core.Data.Relational.Repositories.Implements;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Products;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Products;
using System;

namespace ShopFComputerBackEnd.Product.Data.Repositories.Implements.Products
{
    public class ProductRepository : Repository<ProductReadModel>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
