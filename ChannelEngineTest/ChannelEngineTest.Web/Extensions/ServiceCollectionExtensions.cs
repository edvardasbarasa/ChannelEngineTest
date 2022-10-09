using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChannelEngineTest.ChannelEngineExternal.Extensions;
using ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChannelEngineTest.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddMediatR(GetMediatRAssemblies().ToArray())
                .AddChannelEngineService(configuration);
        }

        private static IEnumerable<Assembly> GetMediatRAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(GetTopProductsQuery))!;
        }
    }
}