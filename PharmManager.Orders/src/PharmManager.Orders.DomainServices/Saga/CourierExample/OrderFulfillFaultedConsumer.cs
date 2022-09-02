using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Domain.Services;

namespace PharmManager.Orders.DomainServices.Saga.CourierExample
{
    public class OrderFulfillFaultedConsumer : IConsumer<OrderFulfillFaulted>
    {
        private readonly IOrdersService _orderService;
        private readonly ILogger<OrderFulfillFaultedConsumer> _logger;

        public OrderFulfillFaultedConsumer(IOrdersService orderService, ILogger<OrderFulfillFaultedConsumer> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderFulfillFaulted> context)
        {
            _logger.LogInformation("Order Faulted Courier called for order {orderId}", context.Message.OrderId);

            await _orderService.RejectOrderAsync(context.Message.OrderId);
        }
    }
}
