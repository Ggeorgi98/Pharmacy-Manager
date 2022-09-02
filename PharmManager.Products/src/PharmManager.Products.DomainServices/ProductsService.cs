using PharmManager.Products.Domain.Dtos;
using PharmManager.Products.Domain.Repositories;
using PharmManager.Products.Domain.Services;
using PharmManager.Products.DomainServices;

namespace BookingApp.Users.DomainServices
{
    public class ProductsService : BaseCrudService<ProductDto>, IProductsService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository)
            : base(productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task TakeProductsCount(Dictionary<Guid, int> productsCount)
        {
            await _productsRepository.TakeProductsCount(productsCount);
        }

        public async Task ReturnProductsCount(Dictionary<Guid, int> productsCount)
        {
            await _productsRepository.ReturnProductsCount(productsCount);
        }
    }
}
