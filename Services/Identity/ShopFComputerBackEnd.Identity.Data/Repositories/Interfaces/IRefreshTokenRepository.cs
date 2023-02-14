using Iot.Core.Data.Relational.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;

namespace ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshTokenReadModel>
    {
    }
}
