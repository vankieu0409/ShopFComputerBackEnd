using MediatR;

using ShopFComputerBackEnd.Cart.Domain.Dtos;

using System.Linq;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Queries
{
    public class GetCartQuery : IRequest<IQueryable<CartDto>>
    {
    }
}
