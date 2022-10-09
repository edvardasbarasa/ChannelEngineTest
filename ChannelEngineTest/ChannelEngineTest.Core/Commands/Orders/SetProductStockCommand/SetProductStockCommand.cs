using FluentResults;
using MediatR;

namespace ChannelEngineTest.Core.Commands.Orders.SetProductStockCommand
{
    public class SetProductStockCommand : IRequest<Result>
    {
        public int Stock { get; set; }
        
        public string MerchantProductNo { get; set; }
    }
}