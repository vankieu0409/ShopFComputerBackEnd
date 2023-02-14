
using AutoMapper.Extensions.ExpressionMapping;
using Iot.Core.DependencyInjection.Extensions;
using Iot.Core.EventBus.Configurations;
using Iot.Core.EventBus.Extensions.DependencyInjection;
using Iot.Core.EventStore.Configurations;
using Iot.Core.EventStore.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Profile.Data;
using ShopFComputerBackEnd.Profile.Data.Repositories.Implements;
using ShopFComputerBackEnd.Profile.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using ShopFComputerBackEnd.Profile.Infrastructure.Handlers.IntegrationEvents;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var entryAssembly = Assembly.GetEntryAssembly();
                //var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                services.AddDependencies();
                services.AddAutoMapper(configuration =>
                {
                    configuration.AddExpressionMapping();
                }, executingAssembly, entryAssembly);
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
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
                services.AddMediatR(executingAssembly, entryAssembly);
                services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
                services.AddEventStore(configuration.GetSection("EventStore").Get<EventStoreConfiguration>());
                services.AddEventBus(configuration.GetSection("EventBus").Get<EventBusConfiguration>());
                services.AddTransient<IProfileRepository, ProfileRepository>();
                services.AddTransient<UserIntegrationEventHandler>();
                return services;
            }
        }
    }
}
