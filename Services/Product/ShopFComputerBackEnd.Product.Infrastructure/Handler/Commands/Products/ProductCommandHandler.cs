using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Products;
using ShopFComputerBackEnd.Product.Domain.Aggregates;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using ShopFComputerBackEnd.Product.Infrastructure.Commands.Products;

namespace ShopFComputerBackEnd.Product.Infrastructure.Handler.Commands.Products
{
    public class ProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>,
        IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<ProductAggregateroot> _eventStore;
        private readonly ILogger<ProductCommandHandler> _logger;
        public ProductCommandHandler(IMapper mapper, IEventStoreService<ProductAggregateroot> eventStore,
            ILogger<ProductCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Product

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductCommandHandler - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProductAggregateroot(request.Id);
            aggregateRoot.Initialize(request.Name,request.Description,request.Category, request.Brand, request.ProductVariants, request.Options, request.CreatedBy);

            _logger.LogDebug($"ProductCommandHandler - Create to EvenStore - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);

            _logger.LogDebug($"ProductCommandHandler - {request.Id} - Handle End {nameof(request)}");

            return _mapper.Map<ProductDto>(aggregateRoot);
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductCommandHandler - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProductAggregateroot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            _logger.LogDebug($"ProductCommandHandler - Property : Name - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.Name, request.Name))
                aggregateStream.UpdateProductName(request.Name, request.ModefiledBy);
            _logger.LogDebug($"ProductCommandHandler - Property : Name - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.Description, request.Description))
                aggregateStream.UpdateProductDescription(request.Description, request.ModefiledBy);
            _logger.LogDebug($"ProductCommandHandler - Property : Name - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.Category, request.Category))
                aggregateStream.UpdateProductCategory(request.Category, request.ModefiledBy);
            _logger.LogDebug($"ProductCommandHandler - Property : Name - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.Brand, request.Brand))
                aggregateStream.UpdateProductBrand(request.Brand, request.ModefiledBy);

            _logger.LogDebug($"ProductCommandHandler - Property : IsDeleted - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!Equals(aggregateStream.IsDeleted, request.IsDeleted))
                aggregateStream.ProductDisable(request.IsDeleted, request.ModefiledBy);

            _logger.LogDebug($"ProductCommandHandler - Property : ProductVariants - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (aggregateStream.ProductVariants.Except(request.ProductVariants).Any())
                aggregateStream.UpdateProductVariants(request.ProductVariants, request.ModefiledBy);

            _logger.LogDebug($"ProductCommandHandler - Update To EvenStore - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.AppendStreamAsync(aggregateRoot.StreamName, aggregateRoot);

            _logger.LogDebug($"ProductCommandHandler - {request.Id} - Handle End {nameof(request)}");

            return _mapper.Map<ProductDto>(aggregateStream);
        }

        #endregion
    }
}
