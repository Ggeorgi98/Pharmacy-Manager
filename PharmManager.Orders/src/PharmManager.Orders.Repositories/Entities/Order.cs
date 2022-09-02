using PharmManager.Orders.Contracts;

namespace PharmManager.Orders.Repositories.Entities
{
    public class Order : BaseEntityWithId
    {
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public OrderState State { get; set; }
    }
}
