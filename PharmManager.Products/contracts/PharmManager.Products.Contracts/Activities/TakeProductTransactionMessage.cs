namespace PharmManager.Products.Contracts.Messages
{
    public interface TakeProductTransactionMessage
    {
        Dictionary<Guid, int> ProductsCount { get; }
    }
}