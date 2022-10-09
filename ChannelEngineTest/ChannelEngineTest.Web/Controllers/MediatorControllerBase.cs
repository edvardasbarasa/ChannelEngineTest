using System.Linq;
using ChannelEngineTest.Infrastructure.Extensions;
using ChannelEngineTest.Web.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelEngineTest.Web.Controllers
{
    public abstract class MediatorControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected MediatorControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        protected IActionResult ProcessErrors<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);

            var errors = result
                .Errors
                .FlattenErrors()
                .ToArray();

            var apiErrors = errors
                .Select(i => new ApiError(i.Message, i.GetPropertyName())).ToList();

            return new ApiBadResponse(apiErrors, StatusCodes.Status400BadRequest);
        }
        
        protected IActionResult processResult(Result result)
        {
            if (result.IsSuccess)
                return Ok();

            var errors = result
                .Errors
                .FlattenErrors()
                .ToArray();

            var apiErrors = errors
                .Select(i => new ApiError(i.Message, i.GetPropertyName())).ToList();

            return new ApiBadResponse(apiErrors, StatusCodes.Status400BadRequest);
        }
    }
}