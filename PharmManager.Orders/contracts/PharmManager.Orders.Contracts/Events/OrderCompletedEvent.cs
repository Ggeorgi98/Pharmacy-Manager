namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderCompletedEvent
    {
        public Guid CorrelationId { get; }
        public Guid OrderId { get; }
    }
}
