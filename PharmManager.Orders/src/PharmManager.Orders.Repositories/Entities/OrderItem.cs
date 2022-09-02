namespace PharmManager.Orders.Repositories.Entities
{
    public class OrderItem : BaseEntityWithId
    {

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Count { get; set; }

        public Order Order { get; set; }
    }
}
