using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ChannelEngineTest.Core.Externals;
using ChannelEngineTest.Core.Models;
using ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChannelEngineTest.Core.Tests.Queries
{
    public class GetTopProductsHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldThrowException_OnException(
            [Frozen] Mock<IChannelEngineService> channelEngineServiceMock,
            GetTopProductsQuery query,
            GetTopProductsHandler handler)
        {
            channelEngineServiceMock.Setup(s =>
                s.GetTopProductsAsync(It.IsAny<int>())).Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }

        [Theory, AutoMoqData]
        public async Task Handle_SouldReturnResultOk_WhenNoException(
            [Frozen] Mock<IChannelEngineService> channelEngineServiceMock,
            GetTopProductsQuery query,
            GetTopProductsHandler handler)
        {
            var products = new List<Product>()
            {
                new Product() {},
                new Product() {},
                new Product() {},
                new Product() {}
            };
            
            channelEngineServiceMock.Setup(s =>
                s.GetTopProductsAsync(It.IsAny<int>())).ReturnsAsync(products);

            var result = await handler.Handle(query, CancellationToken.None);
            
            channelEngineServiceMock.Verify(v => v.GetTopProductsAsync(It.IsAny<int>()), Times.Exactly(1));

            result.IsFailed.Should().BeFalse();
            result.Value.TopProducts.Count.Should().Be(products.Count);
        }
    }
}