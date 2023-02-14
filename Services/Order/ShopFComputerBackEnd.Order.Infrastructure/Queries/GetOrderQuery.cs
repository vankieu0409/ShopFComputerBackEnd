using MediatR;
using ShopFComputerBackEnd.Order.Domain.Dtos;

using System.Linq;

namespace ShopFComputerBackEnd.Order.Infrastructure.Queries
{
    public class GetOrderQuery : IRequest<IQueryable<OrderDto>>
    {
    }
}
