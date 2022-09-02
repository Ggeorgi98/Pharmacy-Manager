using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmManager.Orders.Contracts;
using PharmManager.Orders.Domain.Dtos;
using PharmManager.Orders.Domain.Repositories;
using PharmManager.Orders.Repositories.Entities;

namespace PharmManager.Orders.Repositories
{
    public class OrdersRepository : BaseCrudRepository<Order, OrderDto>, IOrdersRepository
    {
        public OrdersRepository(IDbContextFactory<OrdersContext> dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        protected override void OnBeforeInsert(Order entity)
        {
            base.OnBeforeInsert(entity);
            entity.OrderDate = DateTime.UtcNow;
            entity.State = OrderState.Created;
        }

        public override async Task<OrderDto> GetByIdAsync(Guid id)
        {
            await using var context = _dbContext.CreateDbContext();

            var entity = await context.Set<Order>()
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id)
                .ConfigureAwait(false);

            return Mapper.Map<Order, OrderDto>(entity!);
        }

        public async Task ChangeOrderStateAsync(Guid orderId, OrderState state)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<Order>();

            var entity = await items
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == orderId)
                .ConfigureAwait(false);

            entity!.State = state;

            context.Entry(entity).State = EntityState.Modified;
            items.Update(entity);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
