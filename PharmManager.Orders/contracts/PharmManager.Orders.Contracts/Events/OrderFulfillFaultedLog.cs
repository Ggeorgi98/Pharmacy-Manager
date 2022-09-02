namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderFulfillFaultedLog
    {
        Guid OrderId { get; }
    }
}
