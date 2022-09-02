using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmManager.Products.Domain.Repositories;

namespace PharmManager.Products.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly IDbContextFactory<ProductsContext> _dbContext;
        protected IMapper Mapper { get; }

        protected BaseRepository(IDbContextFactory<ProductsContext> dBContext, IMapper mapper)
        {
            _dbContext = dBContext ?? throw new ArgumentNullException();
            Mapper = mapper ?? throw new ArgumentNullException();
        }
    }
}
