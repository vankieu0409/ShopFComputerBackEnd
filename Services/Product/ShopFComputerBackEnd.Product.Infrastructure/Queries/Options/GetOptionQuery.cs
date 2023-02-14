using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using System.Linq;

namespace ShopFComputerBackEnd.Product.Infrastructure.Queries.Options
{
    public class GetOptionQuery : IRequest<IQueryable<OptionDto>>
    {
    }
}
