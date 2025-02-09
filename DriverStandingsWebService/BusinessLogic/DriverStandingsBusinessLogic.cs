using DriverStandingsWebService.Models;

namespace DriverStandingsWebService.BusinessLogic
{
    public class DriverStandingsBusinessLogic : IDriverStandingsBusinessLogic
    {
        public IEnumerable<DriverStanding> ProcessStandings(IEnumerable<DriverStanding> standings)
        {
            var _standings = standings.ToList();
            if (_standings == null || _standings.Count == 0)
                return new List<DriverStanding>();

            var sortedStandings = _standings.OrderByDescending(d => d.Season_Points).ToList();

            for (int i = 0; i < sortedStandings.Count; i++)
            {
                sortedStandings[i].POS = i + 1;
            }

            return sortedStandings;
        }
    }
}
