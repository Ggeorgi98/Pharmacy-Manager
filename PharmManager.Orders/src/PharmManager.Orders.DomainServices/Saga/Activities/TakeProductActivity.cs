using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.ActivitiesDtos.TakeProduct;
using PharmManager.Orders.Contracts.Utils;
using PharmManager.Orders.Domain.Services;
using PharmManager.Products.Contracts.Activities;
using PharmManager.Products.Contracts.Messages;

namespace PharmManager.Orders.DomainServices.Saga.Activities
{
    public class TakeProductActivity : IActivity<TakeProductArgument, TakeProductLog>
    {
        private readonly IOrdersService _orderService;
        private readonly ILogger<TakeProductActivity> _logger;
        private readonly IRequestClient<TakeProductTransactionMessage> _requestClient;

        public TakeProductActivity(IOrdersService orderService, ILogger<TakeProductActivity> logger,
            IRequestClient<TakeProductTransactionMessage> requestClient)
        {
            _orderService = orderService;
            _logger = logger;
            _requestClient = requestClient;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<TakeProductLog> context)
        {
            _logger.LogInformation("Compensate Take Product Courier called for order {orderId}", context.Log.OrderId);
            var uri = QueueNames.GetMessageUri(nameof(ReturnProductTransactionMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<ReturnProductTransactionMessage>(new
            {
                ProductBaskets = context.Log.ProductsCount
            });

            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<TakeProductArgument> context)
        {
            _logger.LogInformation("Take Product Courier called for order {orderId}", context.Arguments.OrderId);

            await _requestClient.GetResponse<RequestResult>(new { context.Arguments.ProductsCount });

            return context.Completed(new { context.Arguments.ProductsCount, context.Arguments.OrderId });
        }
    }
}
