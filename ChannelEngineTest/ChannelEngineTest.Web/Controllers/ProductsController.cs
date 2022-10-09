using System.Threading;
using System.Threading.Tasks;
using ChannelEngineTest.Core.Commands.Orders.SetProductStockCommand;
using ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery;
using ChannelEngineTest.Web.Models;
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
        
        [HttpPatch("{merchantProductNo}")]
        public async Task<IActionResult> SetProductStockAsync(
            [FromRoute] string merchantProductNo,
            [FromBody] SetProductStockRequest setProductStockRequest,
            CancellationToken cancellationToken)
        {
            var setProductStockResult =
                await _mediator.Send(new SetProductStockCommand()
                {
                    Stock = setProductStockRequest.Stock,
                    MerchantProductNo = merchantProductNo
                }, cancellationToken);

            return processResult(setProductStockResult);
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