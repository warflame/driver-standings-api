using DriverStandingsWebService.DataAccess;
using Moq;
using Moq.Protected;
using System.Net;

namespace DriverStandingsWebServiceTests
{
    public class DriverStandingsRepositoryTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly DriverStandingsRepository _repository;

        public DriverStandingsRepositoryTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _repository = new DriverStandingsRepository(_httpClient);
        }

        [Fact]
        public async Task GetDriverStandingsAsync_ShouldReturnData_WhenResponseIs200()
        {
            // Arrange
            var jsonResponse = "[{ \"First_Name\": \"Max\",\"Last_Name\": \"Verstappen\", \"Season_Points\": 395}]";
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _repository.FetchDriverStandingsAsync(2023);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Standings);
            var _standings = result.Standings.ToList();
            Assert.Single(_standings);
            Assert.Equal("Max Verstappen", _standings[0].Full_Name);
        }

        [Fact]
        public async Task GetDriverStandingsAsync_ShouldReturnError_WhenResponseIs401()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            // Act
            var result = await _repository.FetchDriverStandingsAsync(2023);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Null(result.Standings);
        }
    }
}
