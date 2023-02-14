using AutoMapper;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Queries
{
    public class RefreshTokenQueryHandler : IRequestHandler<GetTokenByRefreshTokenQuery, RefreshTokenReadModel>
    {
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _repository;

        public RefreshTokenQueryHandler(IMapper mapper, IRefreshTokenRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<RefreshTokenReadModel> Handle(GetTokenByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(entity => string.Equals(entity.RefreshToken, request.RefreshToken));
            return result;

        }
    }
}
