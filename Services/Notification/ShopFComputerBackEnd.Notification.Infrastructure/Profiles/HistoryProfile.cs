using ShopFComputerBackEnd.Notification.Domain.Aggregates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Events.Histories;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using ShopFComputerBackEnd.Profile.Grpc.Protos;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Profiles
{
    class HistoryProfile : AutoMapper.Profile
    {
        public HistoryProfile()
        {
            CreateMap<HistoryAggregate, HistoryDto>();
            CreateMap<HistoryInitializedEvent, HistoryReadModel>();
            CreateMap<HistoryReadModel, HistoryDto>();
            CreateMap<AvatarGrpcDto, AvatarValueObject>().ReverseMap();
            CreateMap<ProfileDetailGrpcDto, ProfileDto>().ForMember(destination => destination.Avatar, opt => opt.MapFrom(src => src.Avatar)).ReverseMap();

        }
    }
}
