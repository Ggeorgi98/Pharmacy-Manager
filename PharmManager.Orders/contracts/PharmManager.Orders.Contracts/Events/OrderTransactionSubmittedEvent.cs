namespace PharmManager.Orders.DomainServices.Saga.CourierExample
{
    public interface OrderTransactionSubmittedEvent
    {
        Guid OrderId { get; }

        Guid CorrelationId { get; }
    }
}
