using Microsoft.Extensions.DependencyInjection;
using PharmManager.Products.Domain.Repositories;
using PharmManager.Products.Repositories;

namespace BookingApp.Users.DAL.Utils.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductsRepository, ProductsRepository>();

            return services;
        }

        public static IServiceCollection AddRepositoriesUtils(this IServiceCollection services)
        {
            return services;
        }
    }
}
