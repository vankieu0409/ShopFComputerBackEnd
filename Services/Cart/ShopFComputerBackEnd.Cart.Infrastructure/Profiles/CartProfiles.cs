using AutoMapper;
using ShopFComputerBackEnd.Cart.Domain.Aggregates;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Domain.Events;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using ShopFComputerBackEnd.Cart.Domain.ValueObjects;
using ShopFComputerBackEnd.Product.Grpc.Protos;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Profiles;

public class CartProfiles:Profile
{
    public CartProfiles()
    {
        #region Commands

        CreateMap<CartAggregateRoot, CartDto>().ForMember(dest => dest.Items, op => op.MapFrom(src => src.CartItems));

        #endregion

        #region Events

        CreateMap<CartInitializedEvent, CartReadModel>().ForMember(dest => dest.Items, op => op.MapFrom(src => src.CartItems));

        #endregion

        #region Queries

        CreateMap<CartReadModel, CartDto>();

        #endregion

        #region Grpc


        #endregion
    }
}