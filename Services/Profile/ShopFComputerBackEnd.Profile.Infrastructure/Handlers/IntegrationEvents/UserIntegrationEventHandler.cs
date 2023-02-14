using AutoMapper;
using Iot.Core.DependencyInjection.Abstraction.Interfaces;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Profile.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Handlers.IntegrationEvents
{
    public class UserIntegrationEventHandler : IIntegrationEventHandler<UserSignUpIntegrationEvent>, ITransientDependency
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IProfileRepository _repository;
        public UserIntegrationEventHandler(IMediator mediator, IMapper mapper, IConfiguration configuration, IProfileRepository repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(UserSignUpIntegrationEvent @event)
        {
            var profileExisted = _repository.AsQueryable().FirstOrDefault(entity => string.Equals(entity.PhoneNumber, @event.PhoneNumber) && bool.Equals(entity.IsDeleted, false));
            var resultString = Regex.Replace(@event.PhoneNumber, @"(\+|\b00|\b)8[4]", "0");

            if (profileExisted.IsNullOrDefault())
            {
                var id = Guid.NewGuid();
                var command = new CreateProfileCommand(id);
                _mapper.Map(@event, command);
                command.PhoneNumber = resultString;
                await _mediator.Send(command);
            }
            else
            {
                var commandUpDate = new UpdateProfileCommand(profileExisted.Id);
                _mapper.Map(profileExisted, commandUpDate);
                _mapper.Map(@event, commandUpDate);
                commandUpDate.PhoneNumber = resultString;
                await _mediator.Send(commandUpDate);
            }

        }
    }
}
