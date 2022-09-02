using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.ActivitiesDtos.TakeProduct;
using PharmManager.Products.Contracts.Messages;
using PharmManager.Products.Domain.Services;

namespace PharmManager.Products.DomainServices.Saga.Consumers
{
    public class TakeProductTransactionConsumer : IConsumer<TakeProductTransactionMessage>
    {
        private readonly ILogger<TakeProductTransactionConsumer> _logger;
        private readonly IProductsService _productsService;

        public TakeProductTransactionConsumer(ILogger<TakeProductTransactionConsumer> logger,
            IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<TakeProductTransactionMessage> context)
        {
            _logger.LogInformation($"Take product called");

            await _productsService.TakeProductsCount(context.Message.ProductsCount);

            await context.RespondAsync<RequestResult>(new { Result = 1 });
        }
    }
}
