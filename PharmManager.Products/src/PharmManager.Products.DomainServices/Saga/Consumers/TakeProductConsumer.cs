using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Products.Domain.Services;

namespace PharmManager.Products.DomainServices.Saga.Consumers
{
    public class TakeProductConsumer : IConsumer<TakeProductMessage>
    {
        private readonly ILogger<TakeProductConsumer> _logger;
        private readonly IProductsService _productsService;

        public TakeProductConsumer(ILogger<TakeProductConsumer> logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<TakeProductMessage> context)
        {
            try
            {
                var orderId = context.Message.OrderId;
                _logger.LogInformation("Take product called for order {orderId}", orderId);
                await _productsService.TakeProductsCount(context.Message.ProductsCount);

                await context.Publish<OrderCompletedEvent>(new
                {
                    context.CorrelationId,
                    context.Message.OrderId
                });
            }
            catch (Exception ex)
            {
                await context.Publish<OrderRejectedEvent>(new
                {
                    context.CorrelationId,
                    context.Message.OrderId,
                    Reason = ex.Message
                });
            }
        }
    }
}
