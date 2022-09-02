namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderFulfillCompletedLog
    {
        Guid OrderId { get; }
    }
}
