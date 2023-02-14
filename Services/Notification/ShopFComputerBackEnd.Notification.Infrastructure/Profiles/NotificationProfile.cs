using AutoMapper;
using ShopFComputerBackEnd.Notification.Domain.Aggregates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Profiles
{
    public class NotificationProfile : AutoMapper.Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationAggregateRoot, NotificationDto>();
            CreateMap<DeviceDto, DeviceReadModel>().ReverseMap();
            CreateMap<NotificationInitializedEvent, NotificationReadModel>();
            CreateMap<NotificationReadModel, NotificationDto>();
        }
    }
}
