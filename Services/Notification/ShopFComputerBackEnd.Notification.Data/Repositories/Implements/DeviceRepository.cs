using Iot.Core.Data.Relational.Repositories.Implements;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using System;

namespace ShopFComputerBackEnd.Notification.Data.Repositories.Implements
{
    public class DeviceRepository : Repository<DeviceReadModel>, IDeviceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DeviceRepository(ApplicationDbContext dbContext) : base(dbContext, dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
