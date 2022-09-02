using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmManager.Orders.Domain.Repositories;

namespace PharmManager.Orders.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly IDbContextFactory<OrdersContext> _dbContext;
        protected IMapper Mapper { get; }

        protected BaseRepository(IDbContextFactory<OrdersContext> dBContext, IMapper mapper)
        {
            _dbContext = dBContext ?? throw new ArgumentNullException();
            Mapper = mapper ?? throw new ArgumentNullException();
        }
    }
}
