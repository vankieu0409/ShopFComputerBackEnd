using AutoMapper;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Events.Histories;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Events
{
    public class HistoryEventHandler : INotificationHandler<HistoryInitializedEvent>
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HistoryEventHandler> _logger;

        public HistoryEventHandler(IHistoryRepository historyRepository, IMapper mapper, ILogger<HistoryEventHandler> logger)
        {
            _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(HistoryInitializedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var history = _mapper.Map<HistoryReadModel>(notification);
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(history)}");

            await _historyRepository.AddAsync(history);
            await _historyRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(history)}");
        }
    }
}
