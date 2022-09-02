namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderAcceptedEvent
    {
        public Guid CorrelationId { get; }
        public Guid OrderId { get; }
        public Dictionary<Guid, int> ProductsCount { get; }
    }
}
