using DriverStandingsWebService.Models;
using System.Net;
using System.Text.Json;

namespace DriverStandingsWebService.DataAccess
{
    public class DriverStandingsRepository : IDriverStandingsRepository
    {
        private readonly HttpClient _httpClient;
        private const string AuthKey = "7303c8ef-d91a-4964-a7e7-78c26ee17ec4";

        public DriverStandingsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthKey}");
        }

        public async Task<DriverStandingsResponse> FetchDriverStandingsAsync(int year)
        {
            string url = $"https://pitwall.redbullracing.com/api/standings/drivers/{year}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new DriverStandingsResponse
                    {
                        StatusCode = response.StatusCode,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}."
                    };
                }

                var data = await response.Content.ReadAsStringAsync();
                var standings = JsonSerializer.Deserialize<List<DriverStanding>>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new DriverStandingsResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Standings = standings ?? new List<DriverStanding>()
                };
            }
            catch (Exception ex)
            {
                return new DriverStandingsResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = $"Error fetching data: {ex.Message}"
                };
            }
        }
    }
}
