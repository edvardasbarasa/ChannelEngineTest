using FluentResults;
using MediatR;

namespace ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery
{
    public class GetTopProductsQuery : IRequest<Result<GetTopProductsResponse>>
    {
        public int Top { get; set; }
    }
}