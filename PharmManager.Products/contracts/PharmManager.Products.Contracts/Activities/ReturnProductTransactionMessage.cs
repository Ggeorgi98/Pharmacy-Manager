namespace PharmManager.Products.Contracts.Activities
{
    public interface ReturnProductTransactionMessage
    {
        Dictionary<Guid, int> ProductsCount { get; }
    }
}
