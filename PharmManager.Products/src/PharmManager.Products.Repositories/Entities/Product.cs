namespace PharmManager.Products.Repositories.Entities
{
    public class Product : BaseEntityWithId
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
