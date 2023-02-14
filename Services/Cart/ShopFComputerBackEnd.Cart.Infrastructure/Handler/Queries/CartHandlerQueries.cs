using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopFComputerBackEnd.Cart.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Infrastructure.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Handler.Queries;

public class CartQueryHandler : IRequestHandler<GetCartByUserIdQuery, CartDto>
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _repository;
    public CartQueryHandler(IMapper mapper, ICartRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.AsQueryable().AsNoTracking().ProjectTo<CartDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => Guid.Equals(entity.ProfileId, request.Id));
        return result;
    }
}