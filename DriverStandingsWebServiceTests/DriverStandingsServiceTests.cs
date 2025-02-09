using DriverStandingsWebService.BusinessLogic;
using DriverStandingsWebService.DataAccess;
using DriverStandingsWebService.Models;
using DriverStandingsWebService.Services;
using Moq;
using System.Net;

namespace DriverStandingsWebServiceTests
{
    public class DriverStandingsServiceTests
    {
        private readonly Mock<IDriverStandingsRepository> _repositoryMock;
        private readonly Mock<IDriverStandingsBusinessLogic> _businessLogicMock;
        private readonly DriverStandingsService _service;

        public DriverStandingsServiceTests()
        {
            _repositoryMock = new Mock<IDriverStandingsRepository>();
            _businessLogicMock = new Mock<IDriverStandingsBusinessLogic>();
            _service = new DriverStandingsService(_repositoryMock.Object, _businessLogicMock.Object);
        }

        [Fact]
        public async Task GetDriverStandingsAsync_ShouldSortByPTS()
        {

            // Arrange

            var un_sorted_standings = new List<DriverStanding>
            {
                new DriverStanding { POS = null, First_Name = "Lewis", Last_Name = "Hamilton", Driver_Country_Code = "British", Season_Team_Name = "Mercedes", Season_Points = 350 },
                new DriverStanding { POS = null, First_Name = "Max", Last_Name = "Verstappen", Driver_Country_Code = "Dutch", Season_Team_Name = "Red Bull", Season_Points = 395 },
                new DriverStanding { POS = null, First_Name = "Charles", Last_Name = "Leclerc", Driver_Country_Code = "Monegasque", Season_Team_Name = "Ferrari", Season_Points = 300 }
            };

            var response = new DriverStandingsResponse
            {
                StatusCode = HttpStatusCode.OK,
                Standings = un_sorted_standings
            };

            _repositoryMock.Setup(r => r.FetchDriverStandingsAsync(It.IsAny<int>()))
                .ReturnsAsync(response);


            var sorted_standings = new List<DriverStanding>
            {
                new DriverStanding { POS = 1, First_Name = "Max", Last_Name = "Verstappen", Driver_Country_Code = "Dutch", Season_Team_Name = "Red Bull", Season_Points = 395 },
                new DriverStanding { POS = 2, First_Name = "Lewis", Last_Name = "Hamilton", Driver_Country_Code = "British", Season_Team_Name = "Mercedes", Season_Points = 350 },
                new DriverStanding { POS = 3, First_Name = "Charles", Last_Name = "Leclerc", Driver_Country_Code = "Monegasque", Season_Team_Name = "Ferrari", Season_Points = 300 }
            };

            _businessLogicMock.Setup(b => b.ProcessStandings(It.IsAny<List<DriverStanding>>()))
                .Returns(sorted_standings);

            // Act
            var result = await _service.GetDriverStandingsAsync(2023);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Standings);
            var _standings = result.Standings.ToList();
            Assert.Equal("Max Verstappen", _standings[0].Full_Name);
            Assert.Equal("Lewis Hamilton", _standings[1].Full_Name);
            Assert.Equal("Charles Leclerc", _standings[2].Full_Name);
        }

        [Fact]
        public async Task GetDriverStandingsAsync_ShouldReturnError_WhenRepositoryFails()
        {
            // Arrange
            _repositoryMock.Setup(r => r.FetchDriverStandingsAsync(It.IsAny<int>()))
                .ReturnsAsync(new DriverStandingsResponse { StatusCode = HttpStatusCode.NotFound });

            // Act
            var result = await _service.GetDriverStandingsAsync(2023);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Null(result.Standings);
        }
    }
}
