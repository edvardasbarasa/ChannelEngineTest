using System.Collections.Generic;

namespace ChannelEngineTest.ChannelEngineExternal.Models
{
    public class SetProductStockRequest
    {
        public List<string> PropertiesToUpdate { get; set; } = new List<string>();

        public List<MerchantProductRequestModel> MerchantProductRequestModels { get; set; } =
            new List<MerchantProductRequestModel>();
    }
}