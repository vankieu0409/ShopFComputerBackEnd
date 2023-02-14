using Iot.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Enums;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users;
using Microsoft.AspNetCore.Builder;

namespace ShopFComputerBackEnd.Identity.Api
{
    public static class Seeder
    {
        internal static async Task SeedAsync(this WebApplication app)
        {
            using (var servicescope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            
            {
                var mediator = servicescope.ServiceProvider.GetService<IMediator>();
                var configuration = servicescope.ServiceProvider.GetService<IConfiguration>();

                var userRoleId = Guid.NewGuid();
                var adminRoleId = Guid.NewGuid();
                var userId = Guid.NewGuid();

                var userRole = configuration.GetValue<string>("Role:User");
                var adminRole = configuration.GetValue<string>("Role:Administrator");

                var roles = await mediator.Send(new GetRoleCollectionQuery());

                if (roles.IsNullOrDefault() || !roles.Any())
                {
                    var createUserRoleCommand = new CreateRoleCommand(userRoleId)
                    {
                        Name = userRole,
                    };
                    await mediator.Send(createUserRoleCommand);

                    var createAdminRoleCommand = new CreateRoleCommand(adminRoleId)
                    {
                        Name = adminRole,
                    };
                    await mediator.Send(createAdminRoleCommand);
                }

                var getAdministratorUserQuery = new GetUserDetailByUserNameQuery("administrator");
                var administratorUser = await mediator.Send(getAdministratorUserQuery);

                if (administratorUser.IsNullOrDefault() || administratorUser.Id.IsNullOrDefault())
                {
                    var createUserCommand = new SignUpCommand(userId)
                    {
                        UserName = "administrator",
                        Password = "htctwmssmsmt"
                    };
                    var admin = await mediator.Send(createUserCommand);

                    var roleForAdmin = new AssignUserToRoleCommand(userId)
                    {
                        RoleName = adminRole,
                        UserName = admin.UserName,
                    };
                    await mediator.Send(roleForAdmin);
                }

                var createFunctionCollection = new Collection<CreateFunctionCommand>();

                var policies = GetPolicies();

                var queryCheckFunction = new CheckFunctionExistedByDictionaryServiceNameAndFunctionNameQuery(policies);
                var resultQueryCheckFunction = await mediator.Send(queryCheckFunction);

                if (!resultQueryCheckFunction.IsNullOrDefault())
                {
                    foreach (var item in resultQueryCheckFunction)
                    {
                        foreach (var value in item.Value)
                        {
                            var functionId = Guid.NewGuid();
                            var command = new CreateFunctionCommand(functionId)
                            {
                                ServiceName = item.Key,
                                FunctionName = value
                            };
                            createFunctionCollection.Add(command);
                        }
                    }
                }
                if (createFunctionCollection.Any())
                {
                    var command = new CreateFunctionCollectionCommand(createFunctionCollection);
                    var createdCollectionResult = await mediator.Send(command);

                    var getRoleDetailByNameQuery = new GetRoleDetailByNameQuery(adminRole);
                    var roleDetail = await mediator.Send(getRoleDetailByNameQuery);

                    var assignPermissionCommandCollection = createdCollectionResult.Select(entity => new AssignPermissionCommand(roleDetail.Id, entity.Id)
                    {
                        Type = PermissionType.Role
                    }).ToList();

                    if (assignPermissionCommandCollection.Any())
                    {
                        var assignPermissionCollectionCommand = new AssignPermissionCollectionCommand(assignPermissionCommandCollection);
                        var assignedPermissionCollectionResult = await mediator.Send(assignPermissionCollectionCommand);
                    }
                }
            }
        }

        private static Dictionary<string, IEnumerable<string>> GetPolicies()
        {
            var result = new Dictionary<string, IEnumerable<string>>();
            var fieldInfos = new List<FieldInfo>();
            var types = Assembly.GetEntryAssembly().GetTypes().Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.EndsWith("Policies")).ToArray();
            var fieldInfoArrayCollection = types.Select(type => type.GetFields(BindingFlags.Public |
                 BindingFlags.Static | BindingFlags.FlattenHierarchy));
            foreach (var fieldInfoArray in fieldInfoArrayCollection)
            {
                var resultValue = new Collection<string>();
                foreach (var fieldInfo in fieldInfoArray)
                {
                    if (fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
                    {
                        var key = fieldInfo.DeclaringType?.FullName;
                        var value = string.Empty;
                        var valueInstance = Convert.ToString(fieldInfo.GetValue(value));
                        resultValue.Add(valueInstance);
                        if (!result.Keys.Any(key => string.Equals(key, fieldInfo.DeclaringType?.FullName)))
                            result.Add(key, resultValue);
                        else
                            result[key] = resultValue;
                    }
                }
            }
            return result;
        }
    }
}
