using AutoMapper;
using Iot.Core.AspNetCore.Http;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopFComputerBackEnd.Api.ViewModels.Products;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using ShopFComputerBackEnd.Product.Domain.Entities;
using ShopFComputerBackEnd.Product.Infrastructure.Commands.Products;
using ShopFComputerBackEnd.Product.Infrastructure.Queries.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductsController(IMediator mediator, IMapper mapper,
            IConfiguration configuration)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        [EnableQuery]
        public async Task<ResultMessageBase> PostAsync(CreateProductViewModel viewModel)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                throw new ArgumentNullException("User id"); 


            if (viewModel.Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(viewModel.Name));

            var command = new CreateProductCommand(Guid.NewGuid(),userId);
            command.Name=viewModel.Name;
            command.Category = viewModel.Category;
            command.Brand=viewModel.Brand;
            command.Description=viewModel.Description;
            //set id for foreach entity in options and productvariant
            command.Options = viewModel.Options.Select(entity => new OptionEntity(Guid.NewGuid()) { Name = entity.Name, DisplayOrder = entity.DisplayOrder, ProductId = command.Id }).ToList();
            command.ProductVariants = viewModel.ProductVariants.Select(entity => new ProductVariantEntity(Guid.NewGuid()) { ProductId = command.Id, SkuId = GenarateSkuId(), ImportPrice = entity.ImportPrice, Price = entity.Price, Quantity = entity.Quantity, OptionValues = entity.OptionValues , Images=entity.Images }).ToList();
            // set optionId foreach entity in options of productvariant
            foreach (var productVariant in command.ProductVariants)
            {
                {
                    foreach (var option in productVariant.OptionValues)
                    {
                        option.Id = Guid.NewGuid();
                        option.ProductVariantId = productVariant.Id;
                        option.OptionId = command.Options.FirstOrDefault(entity => int.Equals(entity.DisplayOrder, option.DisplayOrder)).Id;
                    }
                };
            }
            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ResultMessageBase> GetAsync(ODataQueryOptions<ProductDto> queryOptions)
        {
            var query = new GetProductQuery();
            var results = await _mediator.Send(query);

            var finalResult = queryOptions.ApplyTo(results);

            var odataFeature = HttpContext.ODataFeature();
            return PageResultMessage.Success(finalResult, odataFeature.TotalCount);

        }

        [HttpGet("{key}")]
        [EnableQuery]
        public async Task<ResultMessageBase> GetAsync([FromODataUri] Guid key)
        {
            var query = new GetProductByUserIdQuery(key);
            var result = await _mediator.Send(query);
            return SingleResultMessage.Success(result);
        }

        [HttpGet("{key}/GetVariant/{id}")]
        [EnableQuery]
        public async Task<ResultMessageBase> GetVariantAsync([FromODataUri] Guid key,[FromODataUri] Guid id)
        {
            var query = new GetProductByUserIdQuery(key);
            var result = await _mediator.Send(query);

            result.ProductVariants = result.ProductVariants.Where(entity => Guid.Equals(entity.Id, id)).ToList();

            return SingleResultMessage.Success(result);
        }

        [HttpPut("{key}")]
        [EnableQuery]
        public async Task<ResultMessageBase> PutAsync([FromODataUri] Guid key, UpdateProductViewModel viewModel)
        {
            var userId = Guid.Empty;
            if (!Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out userId))
                throw new ArgumentNullException("UserId");

            if (key.IsNullOrDefault())
                throw new ArgumentNullException(nameof(key));

            var query = new GetProductByUserIdQuery(key);
            var resultQuery = await _mediator.Send(query);

            var command = new UpdateProductCommand(key,userId);
            _mapper.Map(viewModel, command);

            var resultCommand = await _mediator.Send(command);

            return SingleResultMessage.Success(resultCommand);
        }

        private string GenarateSkuId()
        {
            var dateTimeNow = DateTime.Now;
            var skuId = $"FSPR-{(dateTimeNow.Day + dateTimeNow.Month + dateTimeNow.Year + dateTimeNow.Hour + dateTimeNow.Minute + dateTimeNow.Second) * 1000 + dateTimeNow.Millisecond}";
            return skuId;
        }
    }
}
