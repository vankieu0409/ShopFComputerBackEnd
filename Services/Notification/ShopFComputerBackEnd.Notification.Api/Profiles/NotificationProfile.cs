using AutoMapper;
using ShopFComputerBackEnd.Notification.Api.ViewModels.Notifications;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.Profiles
{
    public class NotificationProfile : AutoMapper.Profile
    {
        public NotificationProfile()
        {
            CreateMap<CreateNotificationCommand, CreateNotificationViewModel>().ReverseMap();
            CreateMap<UpdateNotificationCommand, UpdateNotificationViewModel>().ReverseMap();
        }
    }
}
