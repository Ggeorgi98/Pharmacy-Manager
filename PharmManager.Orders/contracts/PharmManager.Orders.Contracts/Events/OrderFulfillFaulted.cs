using MassTransit.Courier.Contracts;

namespace PharmManager.Orders.Contracts.Events
{
    public interface OrderFulfillFaulted : RoutingSlipFaulted
    {
        Guid OrderId { get; }
    }
}
