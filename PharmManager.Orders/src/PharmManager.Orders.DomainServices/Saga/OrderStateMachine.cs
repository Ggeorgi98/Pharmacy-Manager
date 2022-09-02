using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Events;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Contracts.Utils;

namespace PharmManager.Orders.DomainServices.Saga
{
    public class OrderStateMachine :
        MassTransitStateMachine<OrderStateInstance>
    {
        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            InstanceState(x => x.CurrentState);
            ConfigureCorrelationIds();
            
            Initially(
                When(OrderSubmitted)
                .Then(x => x.Saga.OrderId = x.Message!.OrderId)
                .Then(x => logger.LogInformation("Order {orderId} submitted", x.Saga.OrderId))
                .ThenAsync(c => RaiseOrderAcceptedEventAsync(c))
                .TransitionTo(Submitted));

            During(Submitted,
                 When(OrderAccepted)
                 .Then(x => logger.LogInformation("Order {orderId} accepted", x.Saga.OrderId))
                 .ThenAsync(c => TakeProductCommandAsync(c))
                 .TransitionTo(Accepted));

            DuringAny(
                   When(OrderRejected)
                   .Then(x => logger.LogInformation("Order {orderId} rejected! because {reason}",
                        x.Saga.OrderId, x.Message.Reason))
                   .ThenAsync(c => RaiseOrderRejectedEventAsync(c))
                   .TransitionTo(Rejected)
                   .Finalize());

            During(Accepted,
                  When(OrderCompleted)
                  .Then(x => logger.LogInformation("Order {orderId} completed", x.Saga.OrderId))
                  .ThenAsync(c => RaiseOrderCompletedEventAsync(c))
                  .ThenAsync(c => RaiseScheduleOrderNotificationAsync(c))
                  .TransitionTo(Completed)
                  .Finalize());
        }

        private static async Task TakeProductCommandAsync(BehaviorContext<OrderStateInstance, OrderAcceptedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(TakeProductMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);            

            await sendEndpoint.Send<TakeProductMessage>(new
            {
                context.Message.CorrelationId,
                context.Message.OrderId,
                context.Message.ProductsCount
            });
        }

        private static async Task RaiseOrderAcceptedEventAsync(BehaviorContext<OrderStateInstance, OrderSubmittedEvent> context)
        {
            await context.Publish<OrderAcceptedEvent>(new
            {
                context.Message.CorrelationId,
                context.Message.OrderId,
                context.Message.ProductsCount
            });
        }

        private static async Task RaiseOrderCompletedEventAsync(BehaviorContext<OrderStateInstance, OrderCompletedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(OrderFulfillCompleted));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<OrderFulfillCompleted>(new
            {
                context.Message.CorrelationId,
                context.Message.OrderId
            });
        }

        private static async Task RaiseOrderRejectedEventAsync(BehaviorContext<OrderStateInstance, OrderRejectedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(OrderFulfillFaulted));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<OrderFulfillFaulted>(new
            {
                context.Message.CorrelationId,
                context.Message.OrderId
            });
        }

        private static async Task RaiseScheduleOrderNotificationAsync(BehaviorContext<OrderStateInstance, OrderCompletedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(ScheduleOrderNotification));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<ScheduleOrderNotification>(new
            {
                context.Message.CorrelationId,
                context.Message.OrderId,
                DeliveryTime = DateTime.UtcNow.AddMinutes(5),
                ReceiverEmail = "test@gmail.com"
            });
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(x => x.Message.CorrelationId)
                   .SelectId(c => c.Message.CorrelationId));
            Event(() => OrderAccepted, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OrderRejected, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OrderCompleted, x => x.CorrelateById(x => x.Message.CorrelationId));
        }

        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        public State Rejected { get; private set; }
        public State Completed { get; private set; }

        public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; }

        public Event<OrderAcceptedEvent> OrderAccepted { get; private set; }

        public Event<OrderRejectedEvent> OrderRejected { get; private set; }

        public Event<OrderCompletedEvent> OrderCompleted { get; private set; }
    }
}
