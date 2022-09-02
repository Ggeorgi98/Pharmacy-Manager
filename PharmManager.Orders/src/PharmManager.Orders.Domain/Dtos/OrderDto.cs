using PharmManager.Orders.Contracts;

namespace PharmManager.Orders.Domain.Dtos
{
    public class OrderDto : BaseDtoWithId
    {
        public DateTime OrderDate { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        public OrderState State { get; set; }
    }
}
