using MassTransit;
using Microsoft.Extensions.Logging;

namespace PharmManager.Orders.DomainServices.Saga.CourierExample
{
    public class OrderCourierStateMachine : MassTransitStateMachine<OrderStateTransactionInstance>
    {
        private readonly ILogger<OrderCourierStateMachine> _logger;

        public OrderCourierStateMachine(ILogger<OrderCourierStateMachine> logger)
        {
            _logger = logger;
            InstanceState(x => x.CurrentState);
            ConfigureCorrelationIds();
            Initially(
                When(OrderSubmitted)
                    .Then(x => x.Saga.OrderId = x.Message.OrderId)
                    .Then(x => _logger.LogInformation("Order Transaction {x.Saga.OrderId} submitted", x.Saga.OrderId))
                    .Activity(c => c.OfType<OrderTransactionSubmittedActivity>())
                    .TransitionTo(Submitted));
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(x => x.Message.CorrelationId)
                   .SelectId(c => c.Message.CorrelationId));

        }

        public State Submitted { get; private set; }

        public Event<OrderTransactionSubmittedEvent> OrderSubmitted { get; private set; }

    }
}
