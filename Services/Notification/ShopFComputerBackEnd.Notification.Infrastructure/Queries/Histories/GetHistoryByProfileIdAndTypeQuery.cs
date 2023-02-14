using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using MediatR;
using System;
using System.Linq;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Queries.Histories
{
    public class GetHistoryByProfileIdAndTypeQuery : IRequest<IQueryable<HistoryDto>>
    {
        public GetHistoryByProfileIdAndTypeQuery(Guid profileId, NotificationType type)
        {
            ProfileId = profileId;
            Type = type;
        }

        public Guid ProfileId { get; set; }
        public NotificationType Type { get; set; }
    }
}
