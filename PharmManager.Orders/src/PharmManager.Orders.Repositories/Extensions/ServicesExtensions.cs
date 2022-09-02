using Microsoft.Extensions.DependencyInjection;
using PharmManager.Orders.Domain.Repositories;

namespace PharmManager.Orders.Repositories.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrdersRepository, OrdersRepository>();

            return services;
        }

        public static IServiceCollection AddRepositoriesUtils(this IServiceCollection services)
        {
            return services;
        }
    }
}
