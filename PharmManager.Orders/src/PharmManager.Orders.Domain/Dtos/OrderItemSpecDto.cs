namespace PharmManager.Orders.Domain.Dtos
{
    public class OrderItemSpecDto
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; }
    }
}
