using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmManager.Products.Domain.Dtos;
using PharmManager.Products.Domain.Repositories;
using PharmManager.Products.Repositories.Entities;
using PharmManager.Productss.Repositories;

namespace PharmManager.Products.Repositories
{
    public class ProductsRepository : BaseCrudRepository<Product, ProductDto>, IProductsRepository
    {
        public ProductsRepository(IDbContextFactory<ProductsContext> dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task TakeProductsCount(Dictionary<Guid, int> productsCount)
        {
            await using var context = _dbContext.CreateDbContext();
            await using var transaction = context.Database.BeginTransaction();
            var items = context.Set<Product>();
            var products = items
                .AsEnumerable()
                .Where(x => productsCount.ContainsKey(x.Id));

            foreach (var product in products)
            {
                var countToSubstract = productsCount[product.Id];
                if (product.Count < countToSubstract)
                {
                    throw new Exception("Product Count is Exeeded the maximum existance!");
                }
                product.Count -= countToSubstract;
            }

            await context.SaveChangesAsync().ConfigureAwait(false);

            await transaction.CommitAsync().ConfigureAwait(false);
        }

        public async Task ReturnProductsCount(Dictionary<Guid, int> productsCount)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<Product>();
            var products = items
                .Where(x => productsCount.ContainsKey(x.Id));

            foreach (var product in products)
            {
                var countToSubstract = productsCount[product.Id];
                if (product.Count < countToSubstract)
                {
                    throw new Exception("Product Count is Exeeded the maximum existance!");
                }
                product.Count += countToSubstract;

                //context.Entry(product).State = EntityState.Modified;
            }

            //items.Update(products);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
