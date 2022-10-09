using System;
using System.Threading.Tasks;
using ChannelEngineTest.Console.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChannelEngineTest.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = Setup(args);
            
            var scope = provider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ConsoleApplication>().RunAsync();
            
            DisposeServices(provider);
        }

        private static ServiceProvider Setup(string[] args)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            services.AddAppDependencies(configuration);
            services.AddScoped<ConsoleApplication>();
            
            return services.BuildServiceProvider();
        }
        
        private static void DisposeServices(ServiceProvider provider)
        {
            if (provider == null)
            {
                return;
            }
            
            if (provider is IDisposable)
            {
                ((IDisposable)provider).Dispose();
            }
        }
    }
}