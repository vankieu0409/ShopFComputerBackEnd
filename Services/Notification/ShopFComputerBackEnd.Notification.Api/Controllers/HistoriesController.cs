 using AutoMapper;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Histories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public HistoriesController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        [Authorize]
        [EnableQuery]
        public async Task<IQueryable<HistoryDto>> GetAsync([FromHeader] Guid profileId, ODataQueryOptions<HistoryDto> queryOptions)
        {
            var query = new GetHistoryCollectionQuery();
            var queryResult = await _mediator.Send(query);
            var finalResult = queryOptions.ApplyTo(queryResult);
            var result = finalResult.Cast<HistoryDto>();
            return result;
        }

        
    }
}
