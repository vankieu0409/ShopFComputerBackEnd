using AutoMapper;
using Iot.Core.EventBus.Base.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopFComputerBackEnd.Product.Data.Repositories.Implements.Options;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Options;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Products;
using ShopFComputerBackEnd.Product.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Infrastructure.Handler.Events.Products
{
    public class ProductEventHandler : INotificationHandler<ProductInitializedEvent>,
        INotificationHandler<ProductNameUpdatedEvent>,
        INotificationHandler<ProductDisableUpdatedEvent>,
        INotificationHandler<ProductOptionUpdatedEvent>,
        INotificationHandler<ProductVariantsUpdatedEvent>,
        INotificationHandler<ProductOptionValueUpdatedEvent>,
        INotificationHandler<ProductVariantUpdatedEvent>,
        INotificationHandler<ProductVariantAddedEvent>,
        INotificationHandler<ProductVariantDisabledEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOptiontRepository _optionRepository;
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public ProductEventHandler(IProductRepository productRepository, IOptiontRepository optionRepository,
            IOptionValueRepository optionValueRepository, IProductVariantRepository productVariantRepository,
            IMapper mapper, ILogger<ProductEventHandler> logger, IEventBus eventBus)
        {
            _optionValueRepository = optionValueRepository ?? throw new ArgumentNullException(nameof(optionValueRepository));
            _optionRepository = optionRepository ?? throw new ArgumentNullException(nameof(optionRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _productVariantRepository = productVariantRepository ?? throw new ArgumentNullException(nameof(productVariantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(ProductInitializedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - Create Product - {notification.Id} - Start Handle {nameof(notification)}");

            var productReadModel = _mapper.Map<ProductReadModel>(notification);

            _logger.LogDebug($"ProductEventHandler - Create Product To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(productReadModel)}");

            await _productRepository.AddAsync(productReadModel);
            await _productRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(productReadModel)}");
        }

        public async Task Handle(ProductNameUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _productRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _productRepository.UpdateAsync(readModel);
            await _productRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductDisableUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _productRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _productRepository.UpdateAsync(readModel);
            await _productRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductOptionUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _optionRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _optionRepository.UpdateAsync(readModel);
            await _optionRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductVariantsUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _optionRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _optionRepository.UpdateAsync(readModel);
            await _optionRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductOptionValueUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _optionValueRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _optionValueRepository.UpdateAsync(readModel);
            await _optionValueRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductVariantUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _productVariantRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _productVariantRepository.UpdateAsync(readModel);
            await _productVariantRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductVariantAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _productVariantRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _productVariantRepository.UpdateAsync(readModel);
            await _productVariantRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }

        public async Task Handle(ProductVariantDisabledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductEventHandler - {notification.Id} - Start Handle {nameof(notification)}");

            var readModel = await _productVariantRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            _mapper.Map(notification, readModel);

            _logger.LogDebug($"ProductEventHandler - Update To Database - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(readModel)}");

            await _productVariantRepository.UpdateAsync(readModel);
            await _productVariantRepository.SaveChangesAsync();

            _logger.LogDebug($"ProductEventHandler - {notification.Id} - End Handle {nameof(readModel)}");
        }
    }
}
