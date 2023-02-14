using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Queries
{
    public class NotificationQueryHandler : IRequestHandler<GetNotificationCollectionQuery, IQueryable<NotificationDto>>,
                                            IRequestHandler<GetNotificationDetailByIdQuery, NotificationDto>,
                                            IRequestHandler<GetNotificationDetailByContextAndNameAndTypeQuery, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        public NotificationQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<IQueryable<NotificationDto>> Handle(GetNotificationCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = _notificationRepository.AsQueryable().AsNoTracking().ProjectTo<NotificationDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }

        public async Task<NotificationDto> Handle(GetNotificationDetailByIdQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.AsQueryable().AsNoTracking().ProjectTo<NotificationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, request.Id));
        }

        public async Task<NotificationDto> Handle(GetNotificationDetailByContextAndNameAndTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationRepository.AsQueryable().AsNoTracking().ProjectTo<NotificationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => string.Equals(entity.Context, request.Context)&&string.Equals(entity.Name, request.Name)&&entity.Type.Equals(request.Type));
            return result;
        }
    }
}
