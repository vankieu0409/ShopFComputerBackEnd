using Iot.Core.Data.Relational.Repositories.Interfaces;

using ShopFComputerBackEnd.Cart.Domain.ReadModels;

namespace ShopFComputerBackEnd.Cart.Data.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<CartReadModel>
    {
    }
}
