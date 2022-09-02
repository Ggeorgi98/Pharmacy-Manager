using AutoMapper;
using BookingApp.Users.DAL.Utils;
using BookingApp.Users.DAL.Utils.Extensions;
using BookingApp.Users.DomainServices.Utils;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi.Models;
using PharmManager.Products.DomainServices.Saga.Consumers;
using PharmManager.Products.Repositories;
using System.Reflection;

namespace PharmManager.Products.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterDatabaseProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextFactory<ProductsContext>(options =>
            {
                options.UseNpgsql(connectionString, 
                    x=> x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Pharm.ProductsAPI"));
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
                cfg.AddProfile(new ProductMappingProfile());
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

        public static IServiceCollection RegisterMasstransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetAssembly(typeof(TakeProductTransactionConsumer)));
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
