using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Core.AspNetCore.Http;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using ShopFComputerBackEnd.Order.Api.ViewModels;
using ShopFComputerBackEnd.Order.Domain.Entities;
using ShopFComputerBackEnd.Order.Infrastructure.Commands.OrderDetails;
using ShopFComputerBackEnd.Order.Infrastructure.Commands.Orders;

namespace ShopFComputerBackEnd.Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public OrderController(IMapper mapper, IMediator mediator, IConfiguration configuration)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("CreateOder")]
        [EnableQuery]
        public async Task<ResultMessageBase> PostAsync(CreateOrderViewModel viewModel)
        {
            if (viewModel.OrderDetailCollection.IsNullOrDefault())
                throw new ArgumentNullException("viewModel - orderDetail");
            var command = new CreateOrderCommand(Guid.NewGuid());
             _mapper.Map<CreateOrderViewModel,CreateOrderCommand>(viewModel,command);
            long amount = 0; 
            if (viewModel.AmountPay.IsNullOrDefault())
            {
                foreach (var orderDetail in viewModel.OrderDetailCollection)
                {
                    amount = amount + (orderDetail.UnitPrice * orderDetail.Quantity);
                }
                command.AmountPay=amount;
            }
            var result= await _mediator.Send(command);
            var orderDetailCollection= new Collection<OrderDetailEntity>();
            foreach (var orderDetail in viewModel.OrderDetailCollection)
            {
                var orderDetailEntity= new OrderDetailEntity();
                orderDetailEntity.OrderId = command.Id;
                orderDetailEntity = _mapper.Map<OrderDetailEntity>(orderDetail);
                orderDetailCollection.Add(orderDetailEntity);
            }

            var updateOrderDetailCommand = new UpdateOrderDetailsCollectionCommand(command.Id, orderDetailCollection);

            var resultDetail= await _mediator.Send(updateOrderDetailCommand);
            

            return SingleResultMessage.Success(result);
        }
    }
}
