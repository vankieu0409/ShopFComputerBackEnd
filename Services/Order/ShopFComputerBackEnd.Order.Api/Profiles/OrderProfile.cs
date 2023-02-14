using AutoMapper;
using ShopFComputerBackEnd.Order.Api.ViewModels;
using ShopFComputerBackEnd.Order.Domain.Entities;
using ShopFComputerBackEnd.Order.Infrastructure.Commands.Orders;

namespace ShopFComputerBackEnd.Order.Api.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderViewModel,CreateOrderCommand>().ReverseMap();
        CreateMap<OrderDetailViewModel, OrderDetailEntity>().ReverseMap();
    }
}