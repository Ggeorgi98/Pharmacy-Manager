using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.ActivitiesDtos.Submit;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Contracts.Utils;
using PharmManager.Orders.Domain.Services;

namespace PharmManager.Orders.DomainServices.Saga.Activities
{
    public class SubmitOrderActivity : IActivity<SubmitOrderArgument, SubmitOrderLog>
    {
        private readonly IOrdersService _orderService;
        private readonly ILogger<SubmitOrderActivity> _logger;

        public SubmitOrderActivity(IOrdersService orderService, ILogger<SubmitOrderActivity> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<SubmitOrderLog> context)
        {
            _logger.LogInformation("Submit Order Courier compensated called for order {orderId}", context.Log.OrderId);
            await _orderService.ReturnOrderAsync(context.Log.OrderId);

            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<SubmitOrderArgument> context)
        {
            _logger.LogInformation("Submit Order Courier called for order {orderId}", context.Arguments.OrderId);
            await _orderService.SubmitOrderAsync(context.Arguments.OrderId);

            return context.Completed(new { context.Arguments.OrderId });
        }
    }
}
