using System.Threading;
using System.Threading.Tasks;
using ChannelEngineTest.Core.Externals;
using FluentResults;
using MediatR;

namespace ChannelEngineTest.Core.Commands.Orders.SetProductStockCommand
{
    public class SetProductStockHander : IRequestHandler<SetProductStockCommand, Result>
    {
        private readonly IChannelEngineService _channelEngineService;

        public SetProductStockHander(IChannelEngineService channelEngineService)
        {
            _channelEngineService = channelEngineService;
        }

        public async Task<Result> Handle(SetProductStockCommand request,
            CancellationToken cancellationToken)
        {
            await _channelEngineService.SetProductStockAsync(request.MerchantProductNo, request.Stock);

            return Result.Ok();
        }
    }
}