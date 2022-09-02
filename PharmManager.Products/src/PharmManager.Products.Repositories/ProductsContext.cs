using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PharmManager.Products.Repositories.Entities;

namespace PharmManager.Products.Repositories
{
    public class ProductsContext : DbContext
    {
        private const string Schema = "PharmProductDb";

        public DbSet<Product> Products { get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);            

            modelBuilder.Entity<Product>()
                .HasIndex(b => b.Id);
        }
    }

    public class ProductsContextFactory : IDesignTimeDbContextFactory<ProductsContext>
    {
        public ProductsContext CreateDbContext(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "PharmManager.Products.API";
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            var config = new ConfigurationBuilder()
               .SetBasePath(path)
               .AddJsonFile("appsettings.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            return new ProductsContext(optionsBuilder.Options);
        }
    }
}
