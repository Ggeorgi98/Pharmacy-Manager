using PharmManager.Products.Domain.Dtos;

namespace PharmManager.Products.Domain.Services
{
    public interface IProductsService : IBaseCrudService<ProductDto>
    {
        Task TakeProductsCount(Dictionary<Guid, int> productsCount);
        Task ReturnProductsCount(Dictionary<Guid, int> productsCount);
    }
}
