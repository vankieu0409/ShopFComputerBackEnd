using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using ShopFComputerBackEnd.Product.Grpc.Protos;
using ShopFComputerBackEnd.Product.Infrastructure.Queries.Products;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Api.Grpc
{
    public class ProductGrpc : ProductGrpcService.ProductGrpcServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductGrpc(IMediator mediator, IConfiguration configuration, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public override async Task<ProductVariantsGrpcDto> GetProductVariantByIds(GetProductVariantByIdsGrpcQuery request, ServerCallContext context)
        {
            var result = new ProductVariantsGrpcDto();

            var query = new GetProductQuery();
            var resultsQuery = await _mediator.Send(query);

            foreach (var product in resultsQuery)
            {
                var items = product.ProductVariants.ToList().Where(entity => request.Ids.Contains(entity.Id.ToString()))
                    .Select(entity => new ProductVariantGrpcDto()
                    {
                        Id = entity.Id.ToString(),
                        Brand = product.Brand,
                        Category = product.Category,
                        Name = product.Name,
                        Price = entity.Price,
                        Images = entity.Images.FirstOrDefault()?.Url,
                        SkuId = entity.SkuId,
                        OptionValues = string.Join(" - ",entity.OptionValues.Select(str => $"{str.Name}: {str.Value}")),
                        ProductId = product.Id.ToString()
                    });
                result.Value.AddRange(items);
            }


            return result;
        }
    }
}
