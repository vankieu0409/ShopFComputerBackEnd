using AutoMapper;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using Iot.Core.Utilities;
using ShopFComputerBackEnd.Identity.Domain.Aggregates;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users;
using ShopFComputerBackEnd.Identity.Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Commands
{
    public class UserCommandHandler : IRequestHandler<SignUpCommand, UserDto>,
                                      IRequestHandler<AddToRoleCommand, RoleDto>,
                                      IRequestHandler<ChangePasswordCommand, UserDto>,
                                      IRequestHandler<ConfirmOtpCommand, UserDto>,
                                      IRequestHandler<ChangeOtpCommand, UserDto>,
                                      IRequestHandler<ForgotPasswordCommand, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<UserAggregateRoot> _eventStore;
        private readonly ILogger<UserCommandHandler> _logger;
        private readonly IEventBus _eventBus;
        public UserCommandHandler(IEventBus eventBus, IMapper iMapper,
                                  IEventStoreService<UserAggregateRoot> eventStore,
                                  ILogger<UserCommandHandler> logger)
        {
            _mapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<UserDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var random = new RandomStringGenerator(false, false, true, false);

            var otp = random.Generate(6);

            var aggregateRoot = new UserAggregateRoot(request.Id);

            aggregateRoot.Initialize(request.UserName, otp, request.Password, request.Email, request.PhoneNumber, request.CreatedBy);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);

            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            _eventBus.Publish(new UserSignUpIntegrationEvent(aggregateRoot.Id, aggregateRoot.UserName, aggregateRoot.Email, aggregateRoot.PhoneNumber, request.DisplayName, aggregateRoot.CreatedBy));

            return _mapper.Map<UserDto>(aggregateRoot);
        }

        public async Task<UserDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new UserAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);
            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)

                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            var encryptNewPassword = (request.NewPassword.Md5Hash() + aggregateRoot.PasswordSalt).Md5Hash();
            var encryptCurrentPassword = (request.CurrentPassword.Md5Hash() + aggregateRoot.PasswordSalt).Md5Hash();
            if (string.Equals(encryptNewPassword, encryptCurrentPassword))
                throw new NewPasswordSameOldPasswordException("Identity", "NewPasswordSameOldPasswordException", 400, "The password is the same as the old password");

            aggregateStream.ChangePassword(request.UserId, request.UserName, request.CurrentPassword, request.NewPassword);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<UserDto>(aggregateStream);
        }

        public async Task<UserDto> Handle(ConfirmOtpCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.User} - Start Handle {nameof(request)}");

            var aggregateRoot = new UserAggregateRoot(request.Id);

            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.User} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");
            if (!string.Equals(aggregateStream.OtpVerify, request.Otp))
                throw new EntityNotFoundException();
            aggregateStream.ConfirmOtp(request.User, request.Otp, request.OtpType);
            _logger.LogDebug($"Command - {request.User} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.User} - End Handle {nameof(request)}");

            return _mapper.Map<UserDto>(aggregateStream);
        }

        public async Task<UserDto> Handle(ChangeOtpCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new UserAggregateRoot(request.Id);

            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");
            if (string.Equals(aggregateStream.OtpVerify, request.Otp))
                throw new EntityAlreadyExistsException();
            aggregateStream.ChangeOtp(request.Otp, request.ModifiedBy);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<UserDto>(aggregateStream);
        }

        public async Task<RoleDto> Handle(AddToRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start handle refresh token");
            var user = new UserAggregateRoot(request.Id);

            var aggregateStream = await _eventStore.AggregateStreamAsync(user.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle refresh token \n Current User: \n {JsonSerializer.Serialize(aggregateStream)}");

            aggregateStream.AddToRole(request.RoleName);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");
            return _mapper.Map<RoleDto>(aggregateStream);
        }

        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<UserDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new UserAggregateRoot(request.Id);

            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            aggregateStream.ForgotPassword(request.NewPassword);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<UserDto>(aggregateStream);
        }
    }
}
