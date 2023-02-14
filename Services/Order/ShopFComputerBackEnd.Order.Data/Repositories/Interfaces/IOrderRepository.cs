using Iot.Core.Data.Relational.Repositories.Interfaces;
using ShopFComputerBackEnd.Order.Domain.ReadModels;

namespace ShopFComputerBackEnd.Order.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<OrderReadModel>
    {
    }
}
