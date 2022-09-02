using PharmManager.Orders.Contracts;
using PharmManager.Orders.Domain.Dtos;

namespace PharmManager.Orders.Domain.Repositories
{
    public interface IOrdersRepository : IBaseCrudRepository<OrderDto>
    {
        Task ChangeOrderStateAsync(Guid orderId, OrderState state);
    }
}
