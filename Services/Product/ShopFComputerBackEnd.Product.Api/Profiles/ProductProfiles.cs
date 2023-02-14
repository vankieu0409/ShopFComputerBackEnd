using AutoMapper;
using ShopFComputerBackEnd.Api.ViewModels.Products;
using ShopFComputerBackEnd.Product.Infrastructure.Commands.Products;

namespace ShopFComputerBackEnd.Product.Api.Profiles
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<UpdateProductViewModel, UpdateProductCommand>();
        }
    }
}
