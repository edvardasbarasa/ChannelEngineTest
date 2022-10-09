using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChannelEngineTest.ChannelEngineExternal.Models;
using ChannelEngineTest.Core.Externals;
using ChannelEngineTest.Core.Models;
using Microsoft.Extensions.Options;
using Polly;

namespace ChannelEngineTest.ChannelEngineExternal.Decorators
{
    public class ChannelEngineServiceRetryDecorator : IChannelEngineService
    {
        private readonly IChannelEngineService _client;
        private readonly ChannelEngineSettings _settings;

        public ChannelEngineServiceRetryDecorator(IChannelEngineService client,
            IOptions<ChannelEngineSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }

        public async Task<IReadOnlyList<Product>> GetTopProductsAsync(int top)
        {
            return await Policy.Handle<QuotaExceededException>()
                .WaitAndRetryAsync(
                    _settings.RetriesCount,
                    i => TimeSpan.FromMilliseconds(i * _settings.ProgressiveRetryDelayMs))
                .ExecuteAsync(async () => await _client.GetTopProductsAsync(
                    top: top));
        }

        public async Task SetProductStockAsync(string merchantProductNo, int stock)
        {
            await _client.SetProductStockAsync(merchantProductNo, stock);
        }
    }
}