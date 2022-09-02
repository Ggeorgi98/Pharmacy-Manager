using Microsoft.Extensions.DependencyInjection;
using PharmManager.Products.Domain.Services;

namespace BookingApp.Users.DomainServices.Utils
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductsService>();

            return services;
        }
    }
}
