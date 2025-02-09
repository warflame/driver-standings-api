using DriverStandingsWebService.Models;

namespace DriverStandingsWebService.BusinessLogic
{
    public interface IDriverStandingsBusinessLogic
    {
        /// <summary>
        /// Process Standings Data to update the POS value
        /// </summary>
        /// <param name="standings"></param>
        /// <returns></returns>
        IEnumerable<DriverStanding> ProcessStandings(IEnumerable<DriverStanding> standings);
    }
}
