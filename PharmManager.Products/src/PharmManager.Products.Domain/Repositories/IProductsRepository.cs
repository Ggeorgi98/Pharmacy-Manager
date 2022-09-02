using PharmManager.Products.Domain.Dtos;

namespace PharmManager.Products.Domain.Repositories
{
    public interface IProductsRepository : IBaseCrudRepository<ProductDto>
    {
        Task TakeProductsCount(Dictionary<Guid, int> productsCount);
        Task ReturnProductsCount(Dictionary<Guid, int> productsCount);
    }
}
