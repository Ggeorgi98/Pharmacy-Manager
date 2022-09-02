namespace PharmManager.Orders.Domain.Dtos
{
    public class OrderItemDto : BaseDtoWithId
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public ProductDto Product { get; set; } = new ProductDto();

        public int Count { get; set; }
    }
}
