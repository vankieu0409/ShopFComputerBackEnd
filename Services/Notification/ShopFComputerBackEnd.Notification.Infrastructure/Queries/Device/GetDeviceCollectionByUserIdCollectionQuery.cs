using ShopFComputerBackEnd.Notification.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Queries.Device
{
    public class GetDeviceCollectionByUserIdCollectionQuery : IRequest<IQueryable<DeviceDto>>
    {
        public GetDeviceCollectionByUserIdCollectionQuery(ICollection<Guid> userIdCollection)
        {
            UserIdCollection = userIdCollection;
        }

        public ICollection<Guid> UserIdCollection { get; set; }
    }
}
