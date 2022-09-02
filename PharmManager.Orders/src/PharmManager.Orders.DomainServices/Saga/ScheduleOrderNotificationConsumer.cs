using MassTransit;
using Microsoft.Extensions.Logging;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Contracts.Utils;

namespace PharmManager.Orders.DomainServices.Saga
{
    public class ScheduleOrderNotificationConsumer : IConsumer<ScheduleOrderNotification>
    {
        private readonly ILogger<ScheduleOrderNotificationConsumer> _logger;

        public ScheduleOrderNotificationConsumer(ILogger<ScheduleOrderNotificationConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ScheduleOrderNotification> context)
        {
            _logger.LogInformation("Scheduling notification message for order with id: {orderId} for {hour}", 
                context.Message.OrderId, context.Message.DeliveryTime);

            var sendOrderNotification = QueueNames.GetMessageUri(nameof(SendOrderNotification));

            await context.ScheduleSend<SendOrderNotification>(sendOrderNotification,
                 context.Message.DeliveryTime,
                 new
                 {
                     context.Message.ReceiverEmail
                 });
        }
    }
}
