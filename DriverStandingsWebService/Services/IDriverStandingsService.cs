using DriverStandingsWebService.Models;

namespace DriverStandingsWebService.Services
{
    public interface IDriverStandingsService
    {
        Task<DriverStandingsResponse> GetDriverStandingsAsync(int year);
    }
}
