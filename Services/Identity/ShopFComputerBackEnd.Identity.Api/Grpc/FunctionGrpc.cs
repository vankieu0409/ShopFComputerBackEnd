using AutoMapper;
using Grpc.Core;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Api.Protos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Api.Grpc
{
    public class FunctionGrpc : FunctionGrpcService.FunctionGrpcServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FunctionGrpc> _logger;
        private readonly IConfiguration _configuration;

        public FunctionGrpc(IMediator mediator, IMapper mapper, ILogger<FunctionGrpc> logger, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async override Task<FunctionCollectionGrpcDto> CreateFunctionCollection(TransferFunctionCollectionGrpcCommand request, ServerCallContext context)
        {
            var result = new FunctionCollectionGrpcDto();
            //var createFunctionCollection = new List<CreateFunctionCommand>();
            //var updateFunctionCollection = new List<UpdateFunctionCommand>();

            var serviceName = request.Value.Select(entity => entity.ServiceName).ToList();
            var functionName = request.Value.Select(entity => entity.FunctionName).ToList();

            var queryCheckFunction = new GetFunctionCollectionByServiceNameCollectionAndFunctionNameCollectionQuery(serviceName, functionName);
            var resultCheckFunction = await _mediator.Send(queryCheckFunction);

            if (resultCheckFunction.FunctionToUpdate.IsNullOrDefault())
            {
                var functionCollectionToUpdate = resultCheckFunction.FunctionToUpdate.Select(entity => new UpdateFunctionCommand(entity.Id)
                {
                    FunctionName = entity.FunctionName,
                    ServiceName = entity.ServiceName,
                }).ToList();
                var commandUpdate = new UpdateFunctionCollectionCommand(functionCollectionToUpdate);
                var updatedCollectionResult = await _mediator.Send(commandUpdate);
                result.Value.AddRange(_mapper.Map<ICollection<FunctionGrpcDto>>(updatedCollectionResult));
            }
            if (resultCheckFunction.FunctionToAdd.IsNullOrDefault())
            {
                var functionCollectionToAdd = resultCheckFunction.FunctionToAdd.Select(entity => new CreateFunctionCommand(Guid.NewGuid())
                {
                    FunctionName = entity.FunctionName,
                    ServiceName = entity.ServiceName,
                }).ToList();
                var commandAdd = new CreateFunctionCollectionCommand(functionCollectionToAdd);
                var createdCollectionResult = await _mediator.Send(commandAdd);
                result.Value.AddRange(_mapper.Map<ICollection<FunctionGrpcDto>>(createdCollectionResult));

                var roleName = _configuration["Role:Administrator"];
                var getRoleDetailByNameQuery = new GetRoleDetailByNameQuery(roleName);
                var roleDetail = await _mediator.Send(getRoleDetailByNameQuery);

                var assignPermissionCommandCollection = createdCollectionResult.Select(entity => new AssignPermissionCommand(roleDetail.Id, entity.Id)
                {
                    Type = PermissionType.Role
                }).ToList();

                #region Old code commented
                //foreach (var item in createdCollectionResult)
                //{
                //    var assignPermissionCommand = new AssignPermissionCommand(roleDetail.Id, item.Id);
                //    assignPermissionCommand.Type = PermissionType.Role;
                //    assignPermissionCommandCollection.Add(assignPermissionCommand);
                //}
                #endregion

                if (assignPermissionCommandCollection.Any())
                {
                    var assignPermissionCollectionCommand = new AssignPermissionCollectionCommand(assignPermissionCommandCollection);
                    var assignedPermissionCollectionResult = await _mediator.Send(assignPermissionCollectionCommand);
                }
            }

            #region Old code commented
            //foreach (var functionRequest in request.Value)
            //{
            //    if (string.IsNullOrEmpty(functionRequest.ServiceName))
            //        continue;
            //    if (string.IsNullOrEmpty(functionRequest.FunctionName))
            //        continue;
            //    try
            //    {
            //        var function = await _mediator.Send(new GetFunctionDetailByServiceAndFuntionQuery(functionRequest.ServiceName, functionRequest.FunctionName));
            //        if (!function.IsNullOrDefault() && !function.Id.IsNullOrDefault())
            //        {
            //            var command = new UpdateFunctionCommand(function.Id);
            //            _mapper.Map(functionRequest, command);
            //            updateFunctionCollection.Add(command);
            //        }
            //        else
            //        {
            //            var id = Guid.NewGuid();
            //            var command = new CreateFunctionCommand(id);
            //            _mapper.Map(functionRequest, command);
            //            createFunctionCollection.Add(command);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, ex.Message);
            //        continue;
            //    }

            //}
            //if (createFunctionCollection.Any())
            //{
            //    var command = new CreateFunctionCollectionCommand(createFunctionCollection);
            //    var createdCollectionResult = await _mediator.Send(command);
            //    result.Value.AddRange(_mapper.Map<ICollection<FunctionGrpcDto>>(createdCollectionResult));

            //    var roleName = _configuration["Role:Administrator"];
            //    var getRoleDetailByNameQuery = new GetRoleDetailByNameQuery(roleName);
            //    var roleDetail = await _mediator.Send(getRoleDetailByNameQuery);

            //    var assignPermissionCommandCollection = new Collection<AssignPermissionCommand>();

            //    foreach (var item in createdCollectionResult)
            //    {
            //        var assignPermissionCommand = new AssignPermissionCommand(roleDetail.Id, item.Id);
            //        assignPermissionCommand.Type = PermissionType.Role;
            //        assignPermissionCommandCollection.Add(assignPermissionCommand);
            //    }

            //    if (assignPermissionCommandCollection.Any())
            //    {
            //        var assignPermissionCollectionCommand = new AssignPermissionCollectionCommand(assignPermissionCommandCollection);
            //        var assignedPermissionCollectionResult = await _mediator.Send(assignPermissionCollectionCommand);
            //    }
            //}

            //if (updateFunctionCollection.Any())
            //{
            //    var command = new UpdateFunctionCollectionCommand(updateFunctionCollection);
            //    var updatedCollectionResult = await _mediator.Send(command);
            //    result.Value.AddRange(_mapper.Map<ICollection<FunctionGrpcDto>>(updatedCollectionResult));
            //}
            #endregion

            return result;
        }

        public async override Task<FunctionCollectionGrpcDto> GetFunctionCollectionByServiceName(GetFunctionCollectionByServiceNameGrpcQuery request, ServerCallContext context)
        {
            var result = new FunctionCollectionGrpcDto();
            var query = new GetFunctionCollectionQuery();
            var function = await _mediator.Send(query);
            result.Value.AddRange(function.Where(f => string.Equals(f.ServiceName, request.ServiceName)).Select(f => _mapper.Map<FunctionGrpcDto>(f)));
            return result;
        }

    }
}
