using AutoMapper.Extensions.ExpressionMapping;
using Iot.Core.DependencyInjection.Extensions;
using Iot.Core.EventBus.Configurations;
using Iot.Core.EventBus.Extensions.DependencyInjection;
using Iot.Core.EventStore.Configurations;
using Iot.Core.EventStore.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Identity.Data;
using ShopFComputerBackEnd.Identity.Data.Repositories.Implements;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Handlers.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var entryAssembly = Assembly.GetEntryAssembly();
                services.AddDependencies();
                services.AddAutoMapper(configuration =>
                {
                    configuration.AddExpressionMapping();
                }, executingAssembly, entryAssembly);

                services.AddMediatR(executingAssembly, entryAssembly);
                services.AddIdentity<ApplicationUserReadModel, ApplicationRoleReadModel>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
                services.AddEventStore(configuration.GetSection("EventStore").Get<EventStoreConfiguration>());
                services.AddEventBus(configuration.GetSection("EventBus").Get<EventBusConfiguration>());
                services.AddDbContext<ApplicationDbContext>(c => c.UseNpgsql(configuration.GetConnectionString("Default")));
                services.AddScoped<IFunctionRepository, FunctionRepository>();
                services.AddScoped<IPermissionRepository, PermissionRepository>();
                services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
                services.AddScoped<IRoleRepository, RoleRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddTransient<PoliciesIntegrationEventHandler>();
                return services;
            }
        }
    }
}
