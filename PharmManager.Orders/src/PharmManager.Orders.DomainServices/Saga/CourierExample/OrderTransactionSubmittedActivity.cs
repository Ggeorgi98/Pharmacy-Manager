using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Contracts.Utils;
using PharmManager.Orders.Domain.Services;

namespace PharmManager.Orders.DomainServices.Saga.CourierExample
{
    internal class OrderTransactionSubmittedActivity :
        IStateMachineActivity<OrderStateTransactionInstance, OrderTransactionSubmittedEvent>
    {
        private readonly ILogger<OrderTransactionSubmittedActivity> _logger;
        private readonly IOrdersService _ordersService;

        public OrderTransactionSubmittedActivity(ILogger<OrderTransactionSubmittedActivity> logger,
            IOrdersService ordersService)
        {
            _logger = logger;
            _ordersService = ordersService;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateTransactionInstance, OrderTransactionSubmittedEvent> context,
            IBehavior<OrderStateTransactionInstance, OrderTransactionSubmittedEvent> next)
        {
            var sendEndpoint = await context.GetSendEndpoint(QueueNames.GetMessageUri(nameof(SubmitOrderMessage)));
            _logger.LogInformation("Order Transaction activity for sendEndpoint {sendEndpoint} will be called", sendEndpoint);
            await sendEndpoint.Send<SubmitOrderMessage>(new
            {
                context.Message.OrderId
            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateTransactionInstance, OrderTransactionSubmittedEvent, TException> context,
            IBehavior<OrderStateTransactionInstance, OrderTransactionSubmittedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submit-order");
        }
    }
}
