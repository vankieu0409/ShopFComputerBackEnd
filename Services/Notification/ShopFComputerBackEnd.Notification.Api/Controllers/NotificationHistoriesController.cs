using AutoMapper;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.Models;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Histories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Profile.Grpc.Protos;

namespace ShopFComputerBackEnd.Notification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationHistoriesController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public NotificationHistoriesController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        [HttpGet("{key}/GetMobileNotification")]
        [Authorize]
        public async Task<IQueryable<HistoryNotificationDtoBase>> GetMobileNotificationAsync([FromODataUri] Guid key, ODataQueryOptions<HistoryDto> queryOptions)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile Id");

            #region Call grpc check profile existed 
            var endpoint = _configuration.GetValue<string>("Services:Profile:Endpoint");
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("Services:Profile:Endpoint");
            var http2UnencryptedSupport = _configuration.GetValue<bool>("Services:Profile:Http2UnencryptedSupport");
            var dangerousAcceptAnyServerCertificateValidator = _configuration.GetValue<bool>("Services:Profile:DangerousAcceptAnyServerCertificateValidator");
            var profile = new ProfileDto();
            using (var httpClientHandler = new SocketsHttpHandler())
            {
                if (dangerousAcceptAnyServerCertificateValidator)
                    httpClientHandler.SslOptions.RemoteCertificateValidationCallback = delegate { return true; };
                httpClientHandler.EnableMultipleHttp2Connections = true;
                httpClientHandler.PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan;
                httpClientHandler.KeepAlivePingDelay = TimeSpan.FromSeconds(60);
                httpClientHandler.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                using (var grpcClientHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClientHandler))
                {
                    using (var channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions { HttpHandler = grpcClientHandler }))
                    {
                        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", http2UnencryptedSupport);
                        var client = new ProfileGrpcService.ProfileGrpcServiceClient(channel);
                        var resultGrpc = await client.GetProfileDetailByIdAsync(new GetProfileDetailByIdGrpcQuery()
                        {
                            Id = key.ToString()
                        });
                        if (resultGrpc.IsNullOrDefault() || !Guid.TryParse(resultGrpc.Id, out var profileId) || profileId.IsNullOrDefault())
                            throw new ArgumentNullException("Profile dose not exist");
                        profile = _mapper.Map<ProfileDto>(resultGrpc);
                    }
                }
            }
            #endregion

            var query = new GetHistoryByProfileIdAndTypeQuery(key, NotificationType.Mobile);
            var result = await _mediator.Send(query);

            var finalResult = queryOptions.ApplyTo(result);

            var castedData = finalResult.Cast<HistoryDto>();
            var resultNotificationCollection = new Collection<HistoryNotificationDtoBase>();
            foreach (var item in castedData)
            {
                var historyNotification = new HistoryNotificationDtoBase();
                var firebaseFcmModel = JsonSerializer.Deserialize<FirebaseFcmModel>(item.Content);
                var payloadJson = firebaseFcmModel.Payload.ToString();

                historyNotification.Payload = JsonSerializer.Deserialize<PayloadDto>(payloadJson);

                historyNotification.Id = Guid.NewGuid();
                historyNotification.Titile = firebaseFcmModel.Notification.Title;
                historyNotification.Content = firebaseFcmModel.Notification.Body;
                historyNotification.Action = item.Action;
                historyNotification.GenealogyName = item.GenealogyName;
                historyNotification.ActionTime = item.SentTime;
                historyNotification.ActionProfile = profile;
                resultNotificationCollection.Add(historyNotification);
            }
            return resultNotificationCollection.AsQueryable();
        }
    }
}
