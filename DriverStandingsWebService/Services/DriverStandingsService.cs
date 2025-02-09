using DriverStandingsWebService.BusinessLogic;
using DriverStandingsWebService.DataAccess;
using DriverStandingsWebService.Models;
using System.Net;

namespace DriverStandingsWebService.Services
{
    public class DriverStandingsService : IDriverStandingsService
    {
        private readonly IDriverStandingsRepository _repository;
        private readonly IDriverStandingsBusinessLogic _businessLogic;

        public DriverStandingsService(IDriverStandingsRepository repository, IDriverStandingsBusinessLogic businessLogic)
        {
            _repository = repository;
            _businessLogic = businessLogic;
        }

        public async Task<DriverStandingsResponse> GetDriverStandingsAsync(int year)
        {
            var response = await _repository.FetchDriverStandingsAsync(year);

            if (response.StatusCode != HttpStatusCode.OK || response.Standings == null)
            {
                return response;
            }

            response.Standings = _businessLogic.ProcessStandings(response.Standings);
            return response;
        }
    }
}
