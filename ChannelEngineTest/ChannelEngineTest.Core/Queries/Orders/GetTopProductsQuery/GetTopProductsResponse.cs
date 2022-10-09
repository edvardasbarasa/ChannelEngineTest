using System.Collections.Generic;
using ChannelEngineTest.Core.Models;

namespace ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery
{
    public class GetTopProductsResponse
    {
        public IReadOnlyList<Product> TopProducts { get; set; } = new List<Product>();
    }
}