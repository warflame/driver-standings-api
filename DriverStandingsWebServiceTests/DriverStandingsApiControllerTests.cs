using DriverStandingsWebService.Controllers;
using DriverStandingsWebService.Models;
using DriverStandingsWebService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace DriverStandingsWebServiceTests
{
    public class DriverStandingsApiControllerTests
    {
        private readonly Mock<IDriverStandingsService> _serviceMock;
        private readonly DriverStandingsApiController _controller;

        public DriverStandingsApiControllerTests()
        {
            _serviceMock = new Mock<IDriverStandingsService>();
            _controller = new DriverStandingsApiController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetData_ShouldReturnOk_WhenDataExists()
        {
            // Arrange
            var standings = new List<DriverStanding> { new() { First_Name = "Max", Last_Name = "Verstappen", Season_Points = 395 } };
            _serviceMock.Setup(s => s.GetDriverStandingsAsync(It.IsAny<int>()))
                .ReturnsAsync(new DriverStandingsResponse { StatusCode = HttpStatusCode.OK, Standings = standings });

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Accept"] = "application/json";

            // Act
            var result = await _controller.GetData(2023);
            var okResult = result.Result as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var data = Assert.IsType<List<DriverStanding>>(okResult.Value);
            Assert.Single(data);
            Assert.Equal("Max Verstappen", data[0].Full_Name);
        }

        [Fact]
        public async Task GetData_ShouldReturn415_WhenAcceptHeaderIsInvalid()
        {
            // Arrange
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.Request.Headers["Accept"] = "text/plain";

            // Act
            var result = await _controller.GetData(2023);
            var okResult = result.Result as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(415, okResult.StatusCode);
        }
    }
}
