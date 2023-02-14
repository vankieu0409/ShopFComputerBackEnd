using AutoMapper;
using ShopFComputerBackEnd.Notification.Api.ViewModels.Templates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.Profiles
{
    public class NotificationTemplateProfile : AutoMapper.Profile
    {
        public NotificationTemplateProfile()
        {
            CreateMap<NotificationTemplateDto, UpdateTemplateViewModel>().ReverseMap();
            CreateMap<NotificationTemplateDto, NotificationTemplateEntity>().ReverseMap();
        }
    }
}
