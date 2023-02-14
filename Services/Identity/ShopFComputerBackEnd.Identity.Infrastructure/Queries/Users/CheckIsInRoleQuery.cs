using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using MediatR;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class CheckIsInRoleQuery : IRequest<bool>
    {
        public CheckIsInRoleQuery(ApplicationUserReadModel user, string roleName)
        {
            User = user;
            RoleName = roleName;
        }

        public ApplicationUserReadModel User { get; set; }
        public string RoleName { get; set; }
    }
}
