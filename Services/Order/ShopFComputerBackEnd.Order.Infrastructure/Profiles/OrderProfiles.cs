using AutoMapper;
using ShopFComputerBackEnd.Order.Domain.Aggregates;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using ShopFComputerBackEnd.Order.Domain.Events.Orders;
using ShopFComputerBackEnd.Order.Domain.ReadModels;
using ShopFComputerBackEnd.Order.Infrastructure.Commands.Orders;

namespace ShopFComputerBackEnd.Order.Infrastructure.Profiles;

public class OrderProfiles : Profile
{
    public OrderProfiles()
    {
        CreateMap<OrderInitializedEvent, OrderReadModel>().ReverseMap();
        CreateMap<CreateOrderCommand,OrderDto>().ReverseMap();
        CreateMap<OrderAggregateRoot,OrderDto>().ReverseMap();
    }
}