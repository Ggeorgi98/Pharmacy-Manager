using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Contracts.Utils;
using PharmManager.Orders.Domain.Services;
using PharmManager.Orders.DomainServices.Saga.Activities;

namespace PharmManager.Orders.DomainServices.Saga
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrderMessage>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;
        private readonly IOrdersService _ordersService;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger, IOrdersService ordersService)
        {
            _logger = logger;
            _ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<SubmitOrderMessage> context)
        {
            var orderDto = await _ordersService.GetByIdAsync(context.Message.OrderId)
                .ConfigureAwait(false);

            var productsCount = orderDto.OrderItems.ToDictionary(x => x.ProductId, v => v.Count);

            _logger.LogInformation("Started submitting order with id: {orderId}", orderDto.Id);

            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var submitOrderUrl = QueueNames.GetActivityUri(nameof(SubmitOrderActivity));
            builder.AddActivity("SubmitOrder", submitOrderUrl, new
            {
                context.Message.OrderId
            });

            builder.AddActivity("TakeProduct", QueueNames.GetActivityUri(nameof(TakeProductActivity)), new
            {
                context.Message.OrderId,
                ProductsCount = productsCount
            });
            builder.AddVariable("OrderId", context.Message.OrderId);

            var orderFaultedAddress = QueueNames.GetMessageUri(nameof(OrderFulfillFaulted));            
            await builder.AddSubscription(orderFaultedAddress,
                RoutingSlipEvents.Faulted, x => x.Send<OrderFulfillFaulted>(new { context.Message.OrderId }));

            var orderFulFilledAddress = QueueNames.GetMessageUri(nameof(OrderFulfillFaulted));
            await builder.AddSubscription(orderFulFilledAddress,
                RoutingSlipEvents.Completed, x => x.Send<OrderFulfillCompleted>(new { context.Message.OrderId }));

            var routingSlip = builder.Build();

            await context.Execute(routingSlip).ConfigureAwait(false);
        }
    }
}
