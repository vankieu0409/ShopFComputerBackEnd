 using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Profile.Domain.Aggregates;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Handlers.Commands
{
    public class ProfileCommandHandler : IRequestHandler<CreateProfileCommand, ProfileDto>,
                                         IRequestHandler<UpdateProfileCommand, ProfileDto>,
                                         IRequestHandler<DeleteProfileCommand, ProfileDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<ProfileAggregateRoot> _eventStore;
        private readonly ILogger<ProfileCommandHandler> _logger;
        public ProfileCommandHandler(IMapper mapper, IEventStoreService<ProfileAggregateRoot> eventStore, ILogger<ProfileCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProfileAggregateRoot(request.Id);
            aggregateRoot.Initialize(request.UserId, request.DisplayName,request.Email, request.PhoneNumber, request.Gender,request.Avatar,request.Address, request.CreatedBy);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<ProfileDto>(aggregateRoot);
        }

        public async Task<ProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProfileAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.DisplayName, request.DisplayName) && !request.DisplayName.IsNullOrDefault())
                aggregateStream.SetDisplayName(request.DisplayName, request.ModifiedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.Email, request.Email) && !request.Email.IsNullOrDefault())
                aggregateStream.SetEmail(request.Email, request.ModifiedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!string.Equals(aggregateStream.PhoneNumber, request.PhoneNumber) && !request.PhoneNumber.IsNullOrDefault())
                aggregateStream.SetPhoneNumber(request.PhoneNumber, request.ModifiedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!Enum.Equals(aggregateStream.Gender, request.Gender))
                aggregateStream.SetGender(request.Gender, request.ModifiedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!AvatarValueObject.Equals(aggregateStream.Avatar, request.Avatar) && !request.Avatar.IsNullOrDefault())
                aggregateStream.SetAvatar(request.Avatar, request.ModifiedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (!AddressValueObject.Equals(aggregateStream.Address, request.Address) && !request.Address.IsNullOrDefault())
                aggregateStream.SetAddress(request.Address, request.ModifiedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<ProfileDto>(aggregateStream);
        }

        public async Task<ProfileDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProfileAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            aggregateStream.Delete(request.DeletedBy);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<ProfileDto>(aggregateStream);
        }

        public async Task<ProfileDto> Handle(RecoverProfileCommand request, CancellationToken cancbellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProfileAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || !aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<ProfileDto>(aggregateStream);
        }

        public async Task<ProfileDto> Handle(UpdateImageCollectionAndGravePhotoCollectionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new ProfileAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<ProfileDto>(aggregateStream);

        }
    }
}
