namespace PharmManager.Orders.Contracts.Messages
{
    public interface SendOrderNotification
    {
        string ReceiverEmail { get; }
    }
}
