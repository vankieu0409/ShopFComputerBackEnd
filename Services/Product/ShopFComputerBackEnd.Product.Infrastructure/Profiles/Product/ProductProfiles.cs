using AutoMapper;
using ShopFComputerBackEnd.Product.Domain.Aggregates;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using ShopFComputerBackEnd.Product.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Products;
using ShopFComputerBackEnd.Product.Infrastructure.Commands.Products;

namespace ShopFComputerBackEnd.Product.Infrastructure.Profiles.Product
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<ProductAggregateroot, ProductDto>();

            CreateMap<ProductInitializedEvent, ProductReadModel>();

            CreateMap<ProductReadModel, ProductDto>();

            CreateMap<ProductVariantReadModel, ProductVariantDto>();

            CreateMap<ProductVariantEntity, ProductVariantReadModel>();
            CreateMap<ProductVariantEntity, ProductVariantDto>();

            CreateMap<OptionValueReadModel, ProductVariantDto>();
            CreateMap<OptionValueReadModel, OptionValueDto>();
            
            CreateMap<OptionReadModel, OptionDto>();
            CreateMap<OptionReadModel, OptionValueDto>();

            CreateMap<OptionEntity, OptionReadModel>();
            CreateMap<OptionEntity, OptionDto>();

            CreateMap<OptionValueEntity, OptionValueReadModel>();
            CreateMap<OptionValueEntity, OptionValueDto>();
        }
    }
}
