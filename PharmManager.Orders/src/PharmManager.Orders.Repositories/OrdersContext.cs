using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PharmManager.Orders.Repositories.Entities;

namespace PharmManager.Orders.Repositories
{
    public class OrdersContext : DbContext
    {
        private const string Schema = "PharmOrderDb";

        public DbSet<Order> Orders { get; set; }

        public OrdersContext(DbContextOptions<OrdersContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);            

            modelBuilder.Entity<Order>()
                .HasIndex(b => b.Id);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithOne(o => o.Order);
        }
    }

    public class ProductsContextFactory : IDesignTimeDbContextFactory<OrdersContext>
    {
        public OrdersContext CreateDbContext(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "PharmManager.Orders.API";
            var optionsBuilder = new DbContextOptionsBuilder<OrdersContext>();
            var config = new ConfigurationBuilder()
               .SetBasePath(path)
               .AddJsonFile("appsettings.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            return new OrdersContext(optionsBuilder.Options);
        }
    }
}
