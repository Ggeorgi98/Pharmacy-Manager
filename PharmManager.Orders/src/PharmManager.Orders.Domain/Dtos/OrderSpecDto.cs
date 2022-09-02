namespace PharmManager.Orders.Domain.Dtos
{
    public class OrderSpecDto
    {
        public ICollection<OrderItemSpecDto> OrderItems { get; set; } = new List<OrderItemSpecDto>();
    }
}
