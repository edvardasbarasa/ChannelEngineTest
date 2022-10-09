using ChannelEngineTest.ChannelEngineExternal.Decorators;
using ChannelEngineTest.ChannelEngineExternal.Models;
using ChannelEngineTest.ChannelEngineExternal.Services;
using ChannelEngineTest.Core.Externals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChannelEngineTest.ChannelEngineExternal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChannelEngineService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ChannelEngineSettings>(options =>
                configuration.GetSection(nameof(ChannelEngineSettings)).Bind(options));

            var baseAddress = configuration.GetSection("ChannelEngineSettings")["BaseAddress"];

            services.AddHttpClient<IChannelEngineService, ChannelEngineService>(c =>
            {
                c.BaseAddress = new System.Uri(baseAddress);
            });
            
            services.Decorate<IChannelEngineService, ChannelEngineServiceRetryDecorator>();

            return services;
        }
    }
}