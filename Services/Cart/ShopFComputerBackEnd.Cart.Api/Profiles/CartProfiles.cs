using AutoMapper;
using ShopFComputerBackEnd.Cart.Api.ViewModels;
using ShopFComputerBackEnd.Cart.Infrastructure.Commands;

namespace ShopFComputerBackEnd.Cart.Api.Profiles;

public class CartProfiles: Profile
{
    public CartProfiles()
    {
        CreateMap<CreateCartViewModel, CreateCartCommand>().ReverseMap();
        CreateMap<UpdateItemCollectionCartViewModel, UpdateCartCommand>();
    }
}