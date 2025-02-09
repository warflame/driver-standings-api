using DriverStandingsWebService.Models;

namespace DriverStandingsWebService.DataAccess
{
    public interface IDriverStandingsRepository
    {
        /// <summary>
        /// Fetch Driver standings details by year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<DriverStandingsResponse> FetchDriverStandingsAsync(int year);
    }
}
