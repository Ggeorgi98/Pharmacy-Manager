namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderFulfillCompleted
    {
        Guid OrderId { get; }
    }
}
