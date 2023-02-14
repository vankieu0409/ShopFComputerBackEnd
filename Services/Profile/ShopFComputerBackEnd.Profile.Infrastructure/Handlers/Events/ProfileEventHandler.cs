using AutoMapper;
using Iot.Core.EventBus.Base.Abstractions;
using ShopFComputerBackEnd.Profile.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Profile.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.ReadModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Handlers.Events
{
    class ProfileEventHandler : INotificationHandler<ProfileInitializedEvent>,
                                INotificationHandler<ProfileDeletedEvent>,
                                INotificationHandler<ProfilePhoneNumberChangedEvent>,
                                INotificationHandler<ProfileEmailChangedEvent>,
                                INotificationHandler<ProfileGenderChangedEvent>,
                                INotificationHandler<ProfileAvatarChangedEvent>,
                                INotificationHandler<ProfileAddressChangedEvent>,
                                INotificationHandler<ProfileDisplayNameChangedEvent>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileEventHandler> _logger;
        private readonly IEventBus _eventBus;
        public ProfileEventHandler(IProfileRepository profileRepository, IMapper mapper,
                                   ILogger<ProfileEventHandler> logger, IEventBus eventBus)
        {
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }
        public async Task Handle(ProfileInitializedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");
            var profile = _mapper.Map<ProfileReadModel>(notification);
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(profile)}");
            await _profileRepository.AddAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(profile)}");
        }
        public async Task Handle(ProfileDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.DeletedBy = notification.DeletedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.RemoveAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(profile)}");
        }
        public async Task Handle(ProfilePhoneNumberChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.PhoneNumber = notification.PhoneNumber;
            profile.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.UpdateAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(profile)}");
        }
        public async Task Handle(ProfileEmailChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start handle profile email changed");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.Email = notification.Email;
            profile.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle profile email changed \n Profile: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.UpdateAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End handle profile email changed");
        }
        public async Task Handle(ProfileGenderChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start handle profile gender changed");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.Gender = notification.Gender;
            profile.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle profile gender changed \n Profile: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.UpdateAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End handle profile gender changed");
        }
        public async Task Handle(ProfileAvatarChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start handle profile avatar changed");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.Avatar = notification.Avatar;
            profile.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle profile avatar changed \n Profile: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.UpdateAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End handle profile avatar changed");
        }
        public async Task Handle(ProfileAddressChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start handle profile address changed");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.Address = notification.Address;
            profile.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle profile address changed \n Profile: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.UpdateAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End handle profile address changed");
        }
        public async Task Handle(ProfileDisplayNameChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start handle profile DisplayName changed");

            var profile = await _profileRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, notification.Id));
            profile.DisplayName = notification.DisplayName;
            profile.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle profile DisplayName changed \n Profile: \n {JsonSerializer.Serialize(profile)}");

            await _profileRepository.UpdateAsync(profile);
            await _profileRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End handle profile DisplayName changed");
        }
    }
}
