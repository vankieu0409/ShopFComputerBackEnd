using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Product.Infrastructure.Exceptions.Products
{
    public class ProductNotFoundException : BusinessException
    {
        public ProductNotFoundException() : base("Product", "ProductNotExitsException",302,"Product not found")
        {
        }
    }
}
