namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderSubmittedEvent
    {
        public Guid OrderId { get; }
        public Guid CorrelationId { get; }
        public Dictionary<Guid, int> ProductsCount { get; }
    }
}
