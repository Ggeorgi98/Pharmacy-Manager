using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using PharmManager.Orders.Domain.Services;
using PharmManager.Orders.Domain.Utils;
using PharmManager.Orders.DomainServices.Services;

namespace PharmManager.Orders.DomainServices.Utils
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddSingleton<IMailClientFactory>(new MailClientFactory(() => new SmtpClient()));

            return services;
        }
    }
}
