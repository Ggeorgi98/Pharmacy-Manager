namespace PharmManager.Orders.Contracts.ActivitiesDtos.TakeProduct
{
    public interface TakeProductArgument
    {
        Guid OrderId { get; }
        Dictionary<Guid, int> ProductsCount { get; }
    }
}
