namespace PharmManager.Orders.Contracts.Messages
{
    public interface ScheduleOrderNotification
    {
        Guid CorrelationId { get; }
        Guid OrderId { get; }
        DateTime DeliveryTime { get; }
        string ReceiverEmail { get; }
    }
}
