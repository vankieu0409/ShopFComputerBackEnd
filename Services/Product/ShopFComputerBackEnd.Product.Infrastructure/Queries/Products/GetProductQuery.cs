using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using System.Linq;

namespace ShopFComputerBackEnd.Product.Infrastructure.Queries.Products
{
    public class GetProductQuery : IRequest<IQueryable<ProductDto>>
    {
    }
}
