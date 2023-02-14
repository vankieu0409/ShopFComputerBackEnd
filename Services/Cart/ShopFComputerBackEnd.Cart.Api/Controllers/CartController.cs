using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Iot.Core.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;
using ShopFComputerBackEnd.Cart.Api.ViewModels;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Infrastructure.Commands;
using ShopFComputerBackEnd.Cart.Infrastructure.Queries;
using System.Threading;
using ShopFComputerBackEnd.Product.Grpc.Protos;
using ShopFComputerBackEnd.Cart.Domain.ValueObjects;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Cart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CartController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        [EnableQuery]
        [Authorize]
        public async Task<ResultMessageBase> GetAsync()
        {
            Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId);
            if (Guid.Equals(userId, Guid.Empty))
                throw new ArgumentNullException(nameof(userId));
            var query = new GetCartByUserIdQuery(userId);

            var result = await _mediator.Send(query);

            var variantIds = result.Items.Select(entity => entity.ProductVariantId+"").ToList();

            #region Call Grpc
            var endpoint = _configuration.GetValue<string>("Services:Product:Endpoint");
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("Services:Product:Endpoint");
            var http2UnencryptedSupport = _configuration.GetValue<bool>("Services:Product:Http2UnencryptedSupport");
            var dangerousAcceptAnyServerCertificateValidator = _configuration.GetValue<bool>("Services:Product:DangerousAcceptAnyServerCertificateValidator");
            using (var httpClientHandler = new SocketsHttpHandler())
            {
                if (dangerousAcceptAnyServerCertificateValidator)
                    httpClientHandler.SslOptions.RemoteCertificateValidationCallback = delegate { return true; };
                httpClientHandler.EnableMultipleHttp2Connections = true;
                httpClientHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpClientHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpClientHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                using (var grpcClientHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClientHandler))
                {
                    using (var channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions { HttpHandler = grpcClientHandler }))
                    {
                        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", http2UnencryptedSupport);
                        // gọi grpc create function
                        var client = new ProductGrpcService.ProductGrpcServiceClient(channel);
                        var grpcQuery = new GetProductVariantByIdsGrpcQuery();
                        grpcQuery.Ids.AddRange(variantIds);
                        var resultGrpc = await client.GetProductVariantByIdsAsync(grpcQuery);
                        result.ItemDetails = new List<ItemsDetailsObjectValue>();
                        foreach (var item in result.Items)
                        {
                            var itemDetail = new ItemsDetailsObjectValue();
                            var detial = resultGrpc.Value.FirstOrDefault(entity => entity.Id.Equals(item.ProductVariantId.ToString()));
                            var variant = result.Items.FirstOrDefault(entity => entity.ProductVariantId.ToString().Equals(detial?.Id));
                            itemDetail.SkuId = detial?.SkuId;
                            itemDetail.Price = (long)detial?.Price;
                            itemDetail.Url = detial?.Images;
                            itemDetail.Brand = detial?.Brand;
                            itemDetail.Caregory = detial?.Category;
                            itemDetail.Name = detial?.Name;
                            itemDetail.OptionValues = detial?.OptionValues;
                            itemDetail.ProductVariantId = variant.ProductVariantId;
                            itemDetail.Quantity = variant.Quantity;
                            result.ItemDetails.Add(itemDetail);
                        }
                    }
                }
            }
            #endregion



            return SingleResultMessage.Success(result);
        }
        
        [HttpPost]
        [EnableQuery]
        [Authorize]
        public async Task<ResultMessageBase> PostAsync([FromBody] CreateCartViewModel viewModel)
        {
            // check userId constrain
            Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId);
            if (Guid.Equals(userId, Guid.Empty))
                throw new ArgumentNullException($"Cart {nameof(userId)}");

            var command = new CreateCartCommand(Guid.NewGuid());
            command = _mapper.Map(viewModel, command);
            command.ProfileId = userId;
            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }
        [HttpPut("{key}")]
        [EnableQuery]
        [Authorize]
        public async Task<ResultMessageBase> PutAsync([FromODataUri] Guid key, [FromBody] UpdateItemCollectionCartViewModel viewModel)
        {
            // check key constrain
            if (Guid.Equals(key, Guid.Empty))
                throw new ArgumentNullException(nameof(key));
            // check userId constrain
            Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId);
            if (Guid.Equals(userId, Guid.Empty))
                throw new ArgumentNullException($"Cart {nameof(userId)}");
            var command = new UpdateCartCommand(key, userId);
            command = _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }
    }
}
