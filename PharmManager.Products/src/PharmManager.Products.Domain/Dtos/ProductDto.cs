namespace PharmManager.Products.Domain.Dtos
{
    public class ProductDto : BaseDtoWithId
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
