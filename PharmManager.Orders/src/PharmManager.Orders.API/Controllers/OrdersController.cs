using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Domain;
using PharmManager.Orders.Domain.Dtos;
using PharmManager.Orders.Domain.Services;
using PharmManager.Orders.DomainServices.Saga.CourierExample;
using PharmManager.Orders.DomainServices.Services;
using System.Net;

namespace PharmManager.Orders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(IOrdersService ordersService, IPublishEndpoint publishEndpoint)
        {
            _ordersService = ordersService;
            _ordersService.ValidationDictionary = new ValidationDictionary(ModelState);
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
        {
            var response = await _ordersService.GetByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResults<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PagedResults<OrderDto>>> GetPagedAllOrders()
        {
            var pager = new Paginator
            {
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var result = await _ordersService.GetListAsync(pager, x => true, null, true);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] OrderSpecDto orderDto)
        {
            var order = await _ordersService.AddAsync(orderDto);

            return Created(order?.Id.ToString()!, Request.Path);
        }

        [HttpPost("submit-order/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SubmitOrder(Guid id)
        {
            await _ordersService.SubmitOrderAsync(id);
            var orderDto = await _ordersService.GetByIdAsync(id)
                .ConfigureAwait(false);

            var productsCount = orderDto.OrderItems.ToDictionary(x => x.ProductId, v => v.Count);

            await _publishEndpoint.Publish<OrderSubmittedEvent>(new
            {
                OrderId = id,
                CorrelationId = Guid.NewGuid(),
                ProductsCount = productsCount
            });

            return Ok();
        }

        [HttpPost("submit-order-transaction-in-state-machine/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SubmitOrderInTransactionWithStateMachine(Guid id)
        {
            await _publishEndpoint.Publish<OrderTransactionSubmittedEvent>(new
            {
                OrderId = id,
                CorrelationId = Guid.NewGuid()
            });

            return Ok();
        }

        [HttpPost("submit-order-transaction/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SubmitOrderInTransaction(Guid id)
        {
            await _publishEndpoint.Publish<SubmitOrderMessage>(new
            {
                OrderId = id,
                CorrelationId = Guid.NewGuid()
            });

            return Ok();
        }
    }
}
