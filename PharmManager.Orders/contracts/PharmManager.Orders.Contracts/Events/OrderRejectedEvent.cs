namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderRejectedEvent
    {
        public Guid CorrelationId { get; }
        public Guid OrderId { get; }
        public string Reason { get; }
    }
}
