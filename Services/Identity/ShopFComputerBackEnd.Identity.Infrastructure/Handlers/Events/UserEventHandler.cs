using AutoMapper;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using Iot.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Enums;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Events.Users;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Commands;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Events
{
    public class UserEventHandler : INotificationHandler<UserInitializedEvent>,
                                    INotificationHandler<UserChangePasswordEvent>,
                                    INotificationHandler<OtpConfirmedEvent>,
                                    INotificationHandler<OtpChangedEvent>,
                                    INotificationHandler<UserForgotPasswordEvent>,
                                    INotificationHandler<UserAddedToRoleEvent>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUserReadModel> _userManager;
        private readonly ILogger<UserCommandHandler> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserEventHandler(IMapper mapper, UserManager<ApplicationUserReadModel> userManager, ILogger<UserCommandHandler> logger, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

        }

        public async Task Handle(UserInitializedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var entity = _mapper.Map<ApplicationUserReadModel>(notification);

            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(entity)}");

            var password = notification.Password.DecryptString("@K*V8s@b4GavqdsM");
            var encryptPassword = (password.Md5Hash() + notification.PasswordSalt).Md5Hash();
            entity.CreatedTime = entity.CreatedTime.Truncate(TimeSpan.FromSeconds(1));
            await _userManager.CreateAsync(entity, encryptPassword);

            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(entity)}");
        }

        public async Task Handle(UserChangePasswordEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");
            var getUserById = new ApplicationUserReadModel();
            var user = await _userManager.FindByIdAsync(notification.Id.ToString());
            if (string.Equals(user.UserName, "administrator"))
                getUserById = await _userManager.FindByIdAsync(notification.UserId.ToString());
            else
                getUserById = await _userManager.FindByIdAsync(notification.Id.ToString());
            var password = notification.NewPassword.DecryptString("@K*V8s@b4GavqdsM");
            var encryptPassword = (password.Md5Hash() + getUserById.PasswordSalt).Md5Hash();

            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(getUserById)}");

            var encryptCurrentPassword = (notification.CurrentPassword.Md5Hash() + getUserById.PasswordSalt).Md5Hash();
            await _userManager.ChangePasswordAsync(getUserById, encryptCurrentPassword, encryptPassword);
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(getUserById)}");
        }

        public async Task Handle(OtpConfirmedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");
            var user = await _userRepository.AsQueryable().FirstOrDefaultAsync(entity => string.Equals(entity.Id, notification.Id));
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(user)}");
            if (!string.Equals(user.OtpVerify, notification.Otp))
                throw new EntityNotFoundException();
            if (OtpType.email.Equals(notification.OtpType))
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(user)}");
        }

        public async Task Handle(OtpChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");
            var user = await _userRepository.AsQueryable().FirstOrDefaultAsync(entity => string.Equals(entity.Id, notification.Id));
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(user)}");
            if (string.Equals(user.OtpVerify, notification.Otp))
                throw new EntityAlreadyExistsException();
            user.OtpVerify = notification.Otp;
            user.ModifiedBy = notification.ModifiedBy;
            await _userManager.UpdateAsync(user);
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(user)}");
        }

        public async Task Handle(UserForgotPasswordEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");
            var user = await _userManager.FindByIdAsync(notification.Id.ToString());

            var password = notification.NewPassword.DecryptString("@K*V8s@b4GavqdsM");
            var encryptPassword = (password.Md5Hash() + user.PasswordSalt).Md5Hash();

            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(user)}");

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPasswordAsync(user, resetPasswordToken, encryptPassword);

            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(notification)}");
        }

        public async Task Handle(UserAddedToRoleEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var user = await _userManager.FindByIdAsync(notification.Id.ToString());
            if (user.IsNullOrDefault())
                throw new ArgumentNullException("User");
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(user)}");
            foreach (var item in notification.RoleName)
            {
                if (await _userManager.IsInRoleAsync(user, item))
                    continue;
                else
                    await _userManager.AddToRoleAsync(user, item);
            }
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(notification)}");
        }
    }
}
