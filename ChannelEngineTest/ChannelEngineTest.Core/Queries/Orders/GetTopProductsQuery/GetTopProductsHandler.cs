using System.Threading;
using System.Threading.Tasks;
using ChannelEngineTest.Core.Externals;
using FluentResults;
using MediatR;

namespace ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery
{
    public class GetTopProductsHandler : IRequestHandler<GetTopProductsQuery, Result<GetTopProductsResponse>>
    {
        private readonly IChannelEngineService _channelEngineService;

        public GetTopProductsHandler(IChannelEngineService channelEngineService)
        {
            _channelEngineService = channelEngineService;
        }

        public async Task<Result<GetTopProductsResponse>> Handle(GetTopProductsQuery request,
            CancellationToken cancellationToken)
        {
            var topProducts = await _channelEngineService.GetTopProductsAsync(request.Top);

            return Result.Ok(new GetTopProductsResponse() { TopProducts = topProducts });
        }
    }
}