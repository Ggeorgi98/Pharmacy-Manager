using MassTransit;
using PharmManager.Products.Contracts.Activities;
using PharmManager.Products.Domain.Services;

namespace PharmManager.Products.DomainServices.Saga.Consumers
{
    public class ReturnProductTransactionConsumer : IConsumer<ReturnProductTransactionMessage>
    {
        private readonly IProductsService _productsService;

        public ReturnProductTransactionConsumer(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ReturnProductTransactionMessage> context)
        {
            await _productsService.ReturnProductsCount(context.Message.ProductsCount);
        }
    }
}
