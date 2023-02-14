using Iot.Core.Data.Relational.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces
{
    public interface INotificationTemplateRepository : IRepository<NotificationTemplateReadModel>
    {
    }
}
