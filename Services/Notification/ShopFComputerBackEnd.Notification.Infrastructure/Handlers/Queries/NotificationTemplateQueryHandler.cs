using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.NotificationTemplates;
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
    public class NotificationTemplateQueryHandler : IRequestHandler<GetNotificationTemplateCollectionByNotificationIdQuery, IQueryable<NotificationTemplateDto>>,
                                                    IRequestHandler<GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery, NotificationTemplateDto>
    {
        private readonly INotificationTemplateRepository _notificationTemplateRepository;
        private readonly IMapper _mapper;
        public NotificationTemplateQueryHandler(INotificationTemplateRepository notificationTemplateRepository, IMapper mapper)
        {
            _notificationTemplateRepository = notificationTemplateRepository ?? throw new ArgumentNullException(nameof(notificationTemplateRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<IQueryable<NotificationTemplateDto>> Handle(GetNotificationTemplateCollectionByNotificationIdQuery request, CancellationToken cancellationToken)
        {
            var result = _notificationTemplateRepository.AsQueryable().AsNoTracking().ProjectTo<NotificationTemplateDto>(_mapper.ConfigurationProvider).Where(entity => Guid.Equals(entity.NotificationId, request.NotificationId));
            return Task.FromResult(result);
        }

        public async Task<NotificationTemplateDto> Handle(GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery request, CancellationToken cancellationToken)
        {
            return await _notificationTemplateRepository.AsQueryable().AsNoTracking().ProjectTo<NotificationTemplateDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => string.Equals(entity.LanguageCode, request.LanguageCode)&&Guid.Equals(entity.NotificationId, request.NotificationId));
        }
    }
}
