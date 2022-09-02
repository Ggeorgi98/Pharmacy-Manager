using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi.Models;
using PharmManager.Orders.API.Utils.Settings;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Contracts.Utils;
using PharmManager.Orders.Domain.Utils;
using PharmManager.Orders.DomainServices.Saga;
using PharmManager.Orders.DomainServices.Saga.CourierExample;
using PharmManager.Orders.DomainServices.Utils;
using PharmManager.Orders.Repositories;
using PharmManager.Orders.Repositories.Extensions;
using PharmManager.Orders.Repositories.Utils;
using System.Reflection;

namespace PharmManager.Orders.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterDatabaseProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextFactory<OrdersContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Pharm.OrdersAPI"));
            });

            return services;
        }

        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddServices();

            return services;
        }

        public static IServiceCollection RegisterSwagger(this IServiceCollection services, string swaggerName)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerName, Version = "v1" });
                c.CustomSchemaIds(type => type.ToString());
                c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "jwt_auth",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()},
                });
            });

            return services;
        }

        public static IServiceCollection RegisterMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrderMappingProfile());
            });

            var mapper = configuration.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection RegisterAuthenticationSettings(this IServiceCollection services)
        {
            services.AddAuthentication();

            return services;
        }

        public static IServiceCollection RegisterMasstransit(this IServiceCollection services, MassTransitConfig massConfig)
        {
            services.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                x.AddConsumers(Assembly.GetAssembly(typeof(OrderFulfillCompletedConsumer)));
                x.AddActivities(Assembly.GetAssembly(typeof(OrderFulfillCompletedConsumer)));
                x.SetKebabCaseEndpointNameFormatter();
                x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
                    .InMemoryRepository();
                x.AddSagaStateMachine<OrderCourierStateMachine, OrderStateTransactionInstance>()
                    .InMemoryRepository();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.UseDelayedMessageScheduler();
                    //cfg.UseMessageScheduler(QueueNames.GetMessageUri(nameof(ScheduleOrderNotification)));
                    cfg.Host(massConfig.Host, massConfig.VirtualHost,
                        h =>
                        {
                            h.Username(massConfig.Username);
                            h.Password(massConfig.Password);
                        }
                    );
                });
            });

            return services;
        }

        public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services;
        }
    }
}
