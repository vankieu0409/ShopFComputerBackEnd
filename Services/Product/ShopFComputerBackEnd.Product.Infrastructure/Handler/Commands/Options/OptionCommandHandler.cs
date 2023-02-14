using AutoMapper;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Options;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using ShopFComputerBackEnd.Product.Infrastructure.Commands.Options;
using ShopFComputerBackEnd.Product.Infrastructure.Exceptions.Products;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Infrastructure.Handler.Commands.Options
{
    public class OptionCommandHandler : IRequestHandler<CreateOptionCommand, OptionDto>,
                                        IRequestHandler<UpdateOptionCommand, OptionDto>,
                                        IRequestHandler<DisableOptionCammand, OptionDto>
    {
        private readonly IMapper _mapper;
        private readonly IOptiontRepository _repository;
        public OptionCommandHandler(IMapper mapper, IOptiontRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<OptionDto> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.AsQueryable().FirstOrDefaultAsync(entity => Equals(entity.Name, request.Name));
            if (result.IsNullOrDefault())
            {
                return _mapper.Map<OptionDto>(result); ;
            }
            var readModel = _mapper.Map<OptionReadModel>(request);

            await _repository.AddAsync(readModel);
            await _repository.SaveChangesAsync();
            return _mapper.Map<OptionDto>(readModel);
        }

        public async Task<OptionDto> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            var readModel = await _repository.AsQueryable().FirstOrDefaultAsync(entity => Equals(entity.Id, request.Id));
            if (readModel.IsNullOrDefault())
                throw new ProductNotFoundException();
            _mapper.Map(request, readModel);

            await _repository.UpdateAsync(readModel);
            await _repository.SaveChangesAsync();
            return _mapper.Map<OptionDto>(readModel);
        }

        public async Task<OptionDto> Handle(DisableOptionCammand request, CancellationToken cancellationToken)
        {
            var readModel = await _repository.AsQueryable().FirstOrDefaultAsync(entity => Equals(entity.Id, request.Id));
            if (readModel.IsNullOrDefault())
                throw new ProductNotFoundException();
            _mapper.Map(request, readModel);


            await _repository.UpdateAsync(readModel);
            await _repository.SaveChangesAsync();
            return _mapper.Map<OptionDto>(readModel);
        }
    }
}
