using PharmManager.Orders.Domain.Dtos;

namespace PharmManager.Orders.Domain.Services
{
    public interface IOrdersService : IBaseCrudService<OrderDto>
    {
        Task SubmitOrderAsync(Guid orderId);
        Task ReturnOrderAsync(Guid orderId);
        Task CloseOrderAsync(Guid orderId);
        Task RejectOrderAsync(Guid orderId);
    }
}
