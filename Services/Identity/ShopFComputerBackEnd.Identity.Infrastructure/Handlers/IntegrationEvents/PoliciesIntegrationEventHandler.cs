using AutoMapper;
using Iot.Core.DependencyInjection.Abstraction.Interfaces;
using Iot.Core.EventBus.Base.Abstractions;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Core.Authentication.Shared.IntegrationEvents;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.IntegrationEvents
{
    public class PoliciesIntegrationEventHandler : IIntegrationEventHandler<PoliciesIntegrationEvent>, ITransientDependency
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFunctionRepository _repository;

        public PoliciesIntegrationEventHandler(IMediator mediator, IMapper mapper, IConfiguration configuration, IFunctionRepository repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(PoliciesIntegrationEvent @event)
        {
            var serviceToAdd = _repository.AsQueryable().Select(entity => entity.ServiceName);
            var functionToAdd = _repository.AsQueryable().Select(entity => entity.FunctionName);

            var policyToAdd = @event.PoliciesCollection.AsQueryable().Where(entity => !serviceToAdd.Contains(entity.ServiceName) && !functionToAdd.Contains(entity.FunctionName)).Select(entity => new FunctionDto()
            {
                FunctionName = entity.FunctionName,
                ServiceName = entity.ServiceName,
            }).ToList();

            var createFunctionCollection = new List<CreateFunctionCommand>();
            foreach (var item in policyToAdd)
            {
                var id = Guid.NewGuid();
                var command = new CreateFunctionCommand(id);
                var result = _mapper.Map(item, command);
                createFunctionCollection.Add(result);
            }
            var commandCreateFunctionCollection = new CreateFunctionCollectionCommand(createFunctionCollection);
            var resultCommand = await _mediator.Send(commandCreateFunctionCollection);

        }
    }
}
