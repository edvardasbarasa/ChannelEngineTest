using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChannelEngineTest.Web.Models
{
    public class ApiBadResponse : IActionResult
    {
        public IReadOnlyList<ApiError> Errors { get; }

        public int HttpStatusCode { get; }
        
        public ApiBadResponse(IReadOnlyList<ApiError> errors,
            int httpStatusCode)
        {
            Errors = errors;

            HttpStatusCode = httpStatusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(this)
            {
                StatusCode = HttpStatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}