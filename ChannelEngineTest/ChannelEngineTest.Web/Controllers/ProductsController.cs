using System.Threading;
using System.Threading.Tasks;
using ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChannelEngineTest.Web.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : MediatorControllerBase
    {
        public ProductsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("top/{top}")]
        public async Task<IActionResult> GetTopProductsAsync([FromRoute] int top,
            CancellationToken cancellationToken)
        {
            var getTopProductsResult =
                await _mediator.Send(new GetTopProductsQuery()
                {
                    Top = top
                }, cancellationToken);

            if (getTopProductsResult.IsSuccess)
            {
                return Ok(getTopProductsResult.Value.TopProducts);
            }

            return ProcessErrors(getTopProductsResult);
        }
    }
}