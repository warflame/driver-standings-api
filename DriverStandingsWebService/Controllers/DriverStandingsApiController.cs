using DriverStandingsWebService.Models;
using DriverStandingsWebService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DriverStandingsWebService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class DriverStandingsApiController : ControllerBase
    {
        private readonly IDriverStandingsService _driverStandingsService;

        public DriverStandingsApiController(IDriverStandingsService driverStandingsService)
        {
            _driverStandingsService = driverStandingsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DriverStanding>>> GetData([FromQuery] int? year)
        {
            var acceptHeader = Request.Headers["Accept"].ToString();

            if (!acceptHeader.Contains("application/json") && !acceptHeader.Contains("application/xml"))
            {
                return StatusCode(415, "Unsupported Media Type. Please use 'application/json' or 'application/xml'.");
            }

            int targetYear = year ?? DateTime.Now.Year;

            var response = await _driverStandingsService.GetDriverStandingsAsync(targetYear);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Standings);
            }

            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
    }
}