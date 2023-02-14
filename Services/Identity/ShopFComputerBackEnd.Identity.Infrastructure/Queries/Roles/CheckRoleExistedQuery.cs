using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using MediatR;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class CheckRoleExistedQuery : IRequest<ApplicationRoleReadModel>
    {
        public CheckRoleExistedQuery(string roleName)
        {
            RoleName = roleName;
        }

        public string RoleName { get; set; }
    }
}
