using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ChannelEngineTest.Core.Commands.Orders.SetProductStockCommand;
using ChannelEngineTest.Core.Externals;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChannelEngineTest.Core.Tests.Commands
{
    public class SetProductStockHanderTests
    {
        [Theory, AutoMoqData]
        public async Task Handle_ShouldThrowException_OnException(
            [Frozen] Mock<IChannelEngineService> channelEngineServiceMock,
            SetProductStockCommand command,
            SetProductStockHander handler)
        {
            channelEngineServiceMock.Setup(s =>
                s.SetProductStockAsync(It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Theory, AutoMoqData]
        public async Task Handle_SouldReturnResultOk_WhenNoException(
            [Frozen] Mock<IChannelEngineService> channelEngineServiceMock,
            SetProductStockCommand command,
            SetProductStockHander handler)
        {
            channelEngineServiceMock.Setup(s =>
                s.SetProductStockAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await handler.Handle(command, CancellationToken.None);

            channelEngineServiceMock.Verify(v => v.SetProductStockAsync(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(1));

            result.IsFailed.Should().BeFalse();
        }
    }
}