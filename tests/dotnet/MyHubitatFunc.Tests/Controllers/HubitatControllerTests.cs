using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using MyHubitatFunc.Controllers;
using Xunit;

namespace MyHubitatFunc.Tests.Controllers
{
    public class HubitatControllerTests
    {
        private const string TestAccessToken = "TestAccessToken";
        private const string TestHubitatConnection = "http://local.hubitat.connection";
        private readonly HubitatController _sut;

        public HubitatControllerTests()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Setup Protected method on HttpMessageHandler mock.
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                    new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    });

            _sut = new HubitatController(
                TestAccessToken,
                new HttpClient(mockHttpMessageHandler.Object),
                TestHubitatConnection
            );
        }

        [Fact]
        public async Task WhenSendCommandItReturnsCommand()
        {
            // Arrange
            var testCommand = "TestCommand";

            // Act
            var command = await _sut.SendCommand(1, testCommand);

            // Assert
            Assert.StartsWith(TestHubitatConnection, command);
            Assert.EndsWith($"={TestAccessToken}", command);
            Assert.EndsWith($"/{testCommand}", command.Split('?')[0]);
        }

        [Fact]
        public async Task WhenSendCommandHasSecondCommandItReturnsCommand()
        {
            // Arrange
            var testCommand = "TestCommand";
            var secondCommand = "SecondCommand";

            // Act
            var command = await _sut.SendCommand(1, testCommand, secondCommand);

            // Assert
            Assert.StartsWith(TestHubitatConnection, command);
            Assert.EndsWith($"={TestAccessToken}", command);
            Assert.EndsWith($"/{secondCommand}", command.Split('?')[0]);
        }
    }
}