using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ShopFComputerBackEnd.Cart.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Cart.Domain.Events;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Handler.Events;

public class CartEventHandler: INotificationHandler<CartInitializedEvent>
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public CartEventHandler(ICartRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw  new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw  new ArgumentNullException(nameof(mapper));
        
    }
    public async Task Handle(CartInitializedEvent notification, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<CartReadModel>(notification);
        await _repository.AddAsync(entity,cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}