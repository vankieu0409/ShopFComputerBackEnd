using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Device;
using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Queries
{
    public class DeviceQueryHandler : IRequestHandler<GetDeviceCollectionByUserIdCollectionQuery, IQueryable<DeviceDto>>
    {
        private readonly IDeviceRepository _repository;
        private readonly IMapper _mapper;

        public DeviceQueryHandler(IDeviceRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<IQueryable<DeviceDto>> Handle(GetDeviceCollectionByUserIdCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.AsQueryable().AsNoTracking().ProjectTo<DeviceDto>(_mapper.ConfigurationProvider).Where(entity => request.UserIdCollection.Contains(entity.UserId));
            return Task.FromResult(result);
        }
    }
}
