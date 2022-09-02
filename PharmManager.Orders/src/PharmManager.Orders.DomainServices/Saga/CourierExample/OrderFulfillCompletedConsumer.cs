using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Domain.Services;

namespace PharmManager.Orders.DomainServices.Saga.CourierExample
{
    public class OrderFulfillCompletedConsumer : IConsumer<OrderFulfillCompleted>
    {
        private readonly IOrdersService _orderService;
        private readonly ILogger<OrderFulfillCompletedConsumer> _logger;

        public OrderFulfillCompletedConsumer(IOrdersService orderService, ILogger<OrderFulfillCompletedConsumer> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderFulfillCompleted> context)
        {
            _logger.LogInformation("Order Completed Courier called for order {orderId}", context.Message);

            await _orderService.CloseOrderAsync(context.Message.OrderId);
        }
    }
}
