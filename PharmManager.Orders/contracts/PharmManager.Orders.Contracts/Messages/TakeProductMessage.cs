namespace PharmManager.Orders.Contracts.Messages
{
    public interface TakeProductMessage
    {
        public Guid CorrelationId { get; }
        public Guid OrderId { get; }
        public Dictionary<Guid, int> ProductsCount { get; }
    }
}
