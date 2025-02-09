using System.Net;

namespace DriverStandingsWebService.Models
{
    public class DriverStandingsResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<DriverStanding>? Standings { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
