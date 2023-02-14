using AutoMapper;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Profiles
{
    public class NotificationTemplateProfile : AutoMapper.Profile
    {
        public NotificationTemplateProfile()
        {
            CreateMap<NotificationTemplateDto, NotificationTemplateReadModel>().ReverseMap();
        }
    }
}
