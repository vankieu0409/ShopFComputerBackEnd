using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Products;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using ShopFComputerBackEnd.Product.Infrastructure.Queries.Products;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Infrastructure.Handler.Queries.Products
{
    public class ProducQueryHandler: IRequestHandler<GetProductQuery, IQueryable<ProductDto>>,
                                     IRequestHandler<GetProductByUserIdQuery, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;
        public ProducQueryHandler(IMapper mapper, IProductRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public Task<IQueryable<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var results = _repository.AsQueryable().AsNoTracking().ProjectTo<ProductDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(results);
        }

        public async Task<ProductDto> Handle(GetProductByUserIdQuery request, CancellationToken cancellationToken)
        {
            var results = await _repository.AsQueryable().AsNoTracking().ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, request.Id));
            return results;
        }
    }
}
