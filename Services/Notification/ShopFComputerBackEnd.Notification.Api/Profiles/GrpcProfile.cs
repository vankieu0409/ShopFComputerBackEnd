using ShopFComputerBackEnd.Notification.Api;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;

namespace ShopFComputerBackEnd.Notification.Api.Profiles
{
    public class GrpcProfile : AutoMapper.Profile
    {
        public GrpcProfile()
        {
            CreateMap<NotificationGrpcDto, NotificationDto>().ReverseMap();
            CreateMap<NotificationGrpcRequest, NotificationBuiltValueObject>().ReverseMap();
            CreateMap<VariableGrpcDto, NotificationVariableValueObject>().ReverseMap();
            CreateMap<NotificationGrpcType, NotificationType>().ReverseMap();
        }
    }
}
