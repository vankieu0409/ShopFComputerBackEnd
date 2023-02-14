using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Histories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ShopFComputerBackEnd.Notification.Domain.Enums.NotificationStatus;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Queries
{
    public class HistoryQueryHandler : IRequestHandler<GetHistoryCollectionQuery, IQueryable<HistoryDto>>,
                                       IRequestHandler<GetHistoryByProfileIdAndTypeQuery, IQueryable<HistoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IHistoryRepository _historyRepository;
        public HistoryQueryHandler(IMapper mapper, IHistoryRepository historyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
        }
        public Task<IQueryable<HistoryDto>> Handle(GetHistoryCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = _historyRepository.AsQueryable().AsNoTracking().ProjectTo<HistoryDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }

        public Task<IQueryable<HistoryDto>> Handle(GetHistoryByProfileIdAndTypeQuery request, CancellationToken cancellationToken)
        {
            var result = _historyRepository.AsQueryable().AsNoTracking().ProjectTo<HistoryDto>(_mapper.ConfigurationProvider).Where(entity => Enum.Equals(entity.Type, request.Type) && string.Equals(entity.Destination, request.ProfileId.ToString()) && entity.Status.Equals(NotificationStatus.Success));
            return Task.FromResult(result);
        }
    }
}
