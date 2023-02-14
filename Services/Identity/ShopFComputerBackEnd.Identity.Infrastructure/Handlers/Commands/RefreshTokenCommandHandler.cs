using AutoMapper;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, RefreshTokenDto>,
                                              IRequestHandler<RevokeRefreshTokenCommand, RefreshTokenDto>
    {
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _repository;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IMapper mapper, IRefreshTokenRepository repository, ILogger<RefreshTokenCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RefreshTokenDto> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var refreshToken = _mapper.Map<RefreshTokenReadModel>(request);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(refreshToken)}");

            await _repository.AddAsync(refreshToken);
            await _repository.SaveChangesAsync();

            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(refreshToken)}");

            return _mapper.Map<RefreshTokenDto>(refreshToken);
        }

        public async Task<RefreshTokenDto> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var refreshTokenExisted = await _repository.AsQueryable().FirstOrDefaultAsync(entity => string.Equals(entity.Id , request.Id));
            var refreshToken = _mapper.Map<RefreshTokenReadModel>(refreshTokenExisted);
            refreshToken.RevokedTime = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(refreshToken);
            await _repository.SaveChangesAsync();

            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<RefreshTokenDto>(refreshToken);

        }
    }
}
