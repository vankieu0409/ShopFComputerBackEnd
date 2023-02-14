using Iot.Core.EventBus.Base.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShopFComputerBackEnd.Core.Authentication.Shared.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace ShopFComputerBackEnd.Core.Authentication.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static void UsePolicies(this IApplicationBuilder applicationBuilder)
        {
            var _eventBus = applicationBuilder.ApplicationServices.GetRequiredService<IEventBus>();
            //đọc và lấy những class có đuôi là Policies và nằm trong namespace là Policies
            var types = Assembly.GetEntryAssembly()
                                        .GetTypes()
                                        .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.EndsWith("Policies")).ToArray();
            //get những giá trị được define => expected list (key, value)
            var policies = GetPolicies(types);
            //call grpc CreateFunctionCollection để insert vào database của identity
            var model = new PolicyBase();
            var modelCollection = new PoliciesIntegrationEvent();
            modelCollection.PoliciesCollection = new Collection<PolicyBase>();
            foreach (var item in policies)
            {
                foreach (var function in item.Value)
                {
                    model = new PolicyBase()
                    {
                        ServiceName = item.Key,
                        FunctionName = function
                    };
                    modelCollection.PoliciesCollection.Add(model);
                }
            }
            _eventBus.Publish(modelCollection);
        }
        private static Dictionary<string, IEnumerable<string>> GetPolicies(IEnumerable<Type> types)
        {
            var result = new Dictionary<string, IEnumerable<string>>();
            var fieldInfos = new List<FieldInfo>();
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