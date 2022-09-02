namespace PharmManager.Orders.Contracts.Messages
{
    public interface SubmitOrderMessage
    {
        Guid OrderId { get; }
    }
}
