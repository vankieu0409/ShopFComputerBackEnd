using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Domain.Enums;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using ShopFComputerBackEnd.Profile.Grpc.Protos;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;
using ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Api.Grpc
{
    public class ProfileGrpc : ProfileGrpcService.ProfileGrpcServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProfileGrpc(IMediator mediator, IConfiguration configuration, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<BoolValue> IsProfileExist(IsProfileExistGrpcQuery request, ServerCallContext context)
        {
            var result = new BoolValue();
            result.Value = false;
            var id = Guid.Empty;
            if (!Guid.TryParse(request.Id, out id))
                return result;
            var query = new IsProfileExistQuery(id);
            result.Value = await _mediator.Send(query);
            return result;
        }

        public override async Task<ProfileDetailGrpcDto> GetProfileDetailById(GetProfileDetailByIdGrpcQuery request, ServerCallContext context)
        {
            var result = new ProfileDetailGrpcDto();
            if (Guid.TryParse(request.Id, out var profileId))
            {
                var query = new GetProfileDetailByIdQuery(profileId);
                var queryResult = await _mediator.Send(query);
                result = new ProfileDetailGrpcDto();

                result.Id = Convert.ToString(queryResult.Id);
                result.UserId = Convert.ToString(queryResult.UserId);
                result.DisplayName = queryResult.DisplayName;
                result.Avatar = _mapper.Map<AvatarGrpcDto>(queryResult.Avatar);
                result.UserId = queryResult.UserId.ToString();
            }
            return result;
        }

        public override async Task<StringValue> CreateProfile(CreateProfileGrpcCommand request, ServerCallContext context)
        {
            var profileId = Guid.NewGuid();
            var command = _mapper.Map<CreateProfileCommand>(request);
            command.Id = profileId;
            command.UserId = string.IsNullOrEmpty(request.UserId) ? Guid.Empty : Guid.Parse(request.UserId);
            await _mediator.Send(command);
            var result = new StringValue();
            result.Value = profileId.ToString();
            return result;
        }

        public override async Task<ProfileDetailGrpcDto> UpdateProfile(UpdateProfileGrpcCommand request, ServerCallContext context)
        {
            var resultCommand = new ProfileDto();
            var resultCommandUpdateMediaUnderProfile = new ProfileDto();
            if (Guid.TryParse(request.Id.ToString(), out var id))
            {
                var command = new UpdateProfileCommand(id);
                command.DisplayName = request.DisplayName;
                command.Avatar = _mapper.Map<AvatarValueObject>(request.Avatar);
                var commandUpdateMediaUnderProfile = new UpdateImageCollectionAndGravePhotoCollectionCommand(id);

                resultCommand = await _mediator.Send(command);
                resultCommandUpdateMediaUnderProfile = await _mediator.Send(commandUpdateMediaUnderProfile);
            }

            var result = new ProfileDetailGrpcDto();
            result.Id = Convert.ToString(resultCommand.Id);
            result.DisplayName = resultCommand.DisplayName ?? "";
            result.Avatar = _mapper.Map<AvatarGrpcDto>(resultCommand.Avatar);
            result.UserId = resultCommand.UserId.ToString();
            return result;
        }
    }
}
