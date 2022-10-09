using System.Collections.Generic;
using System.Threading.Tasks;
using ChannelEngineTest.Core.Models;

namespace ChannelEngineTest.Core.Externals
{
    public interface IChannelEngineService
    {
        Task<IReadOnlyList<Product>> GetTopProductsAsync(int top);

        Task SetProductStockAsync(string merchantProductNo, int stock);
    }
}