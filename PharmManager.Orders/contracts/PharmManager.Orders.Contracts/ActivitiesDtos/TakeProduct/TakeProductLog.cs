namespace PharmManager.Orders.Contracts.ActivitiesDtos.TakeProduct
{
    public interface TakeProductLog
    {
        Guid OrderId { get; }
        Dictionary<Guid, int> ProductsCount { get; }
    }
}
