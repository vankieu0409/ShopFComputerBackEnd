using MediatR;

using ShopFComputerBackEnd.Cart.Domain.Dtos;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Queries
{
    public class GetCartDetailsQuery : IRequest<CartDto>
    {
    }
}
