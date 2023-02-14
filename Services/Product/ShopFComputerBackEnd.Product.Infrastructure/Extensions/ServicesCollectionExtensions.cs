using AutoMapper.Extensions.ExpressionMapping;
using Iot.Core.EventBus.Configurations;
using Iot.Core.EventBus.Extensions.DependencyInjection;
using Iot.Core.EventStore.Configurations;
using Iot.Core.EventStore.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShopFComputerBackEnd.Product.Data;
using ShopFComputerBackEnd.Product.Data.Repositories.Implements.Options;
using ShopFComputerBackEnd.Product.Data.Repositories.Implements.Products;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Options;
using ShopFComputerBackEnd.Product.Data.Repositories.Interfaces.Products;
using ShopFComputerBackEnd.Product.Infrastructure.Handler.Commands.Options;
using ShopFComputerBackEnd.Product.Infrastructure.Handler.Commands.Products;
using System.Reflection;
using System.Text;

namespace ShopFComputerBackEnd.Product.Infrastructure.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var entryAssembly = Assembly.GetEntryAssembly();
            services.AddAutoMapper(configuration =>
            {
                configuration.AddExpressionMapping();
            }, executingAssembly, entryAssembly);
            services.AddMediatR(executingAssembly, entryAssembly);
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:ValidAudience"],
                    ValidIssuer = configuration["Jwt:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])),
                    RequireExpirationTime = false
                };
            });
            services.AddEventBus(configuration.GetSection("EventBus").Get<EventBusConfiguration>());
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
            //services.AddDistributedCache(configuration.GetSection("DistributedCache").Get<DistributedCacheConfiguration>());
            services.AddEventStore(configuration.GetSection("EventStore").Get<EventStoreConfiguration>());
            services.AddEventBus(configuration.GetSection("EventBus").Get<EventBusConfiguration>());

            services.AddTransient<ProductCommandHandler>();
            services.AddTransient<OptionCommandHandler>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IOptiontRepository, OptiontRepository>();
            services.AddScoped<IOptionValueRepository, OptionValueRepository>();

            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            services.AddHttpClient();

            return services;
        }

    }
}
