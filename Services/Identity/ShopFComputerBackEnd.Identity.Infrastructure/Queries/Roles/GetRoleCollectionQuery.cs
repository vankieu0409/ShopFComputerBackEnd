using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleCollectionQuery:IRequest<IQueryable<RoleDto>>
    {
    }
}
