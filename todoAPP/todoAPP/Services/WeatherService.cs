using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public class WeatherService
    {
        private Uri BaseURI;
        private WeatherViewModel? weather;

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public WeatherService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;

            BaseURI = new Uri("https://opendata.cwb.gov.tw");
        }

        async private Task GetWeather()
        {
            Uri apiUri = new Uri(BaseURI, "/api/v1/rest/datastore/O-A0001-001");
            QueryBuilder qBuilder = new QueryBuilder
            {
                { "Authorization", _configuration.GetSection("CWB:Key").Value },
                { "stationId", "C0AC70" },
                { "elementName", "Weather" }
            };
            string queryParamStr = qBuilder.ToQueryString().ToUriComponent();

            Uri queryUri = new Uri(apiUri, queryParamStr);
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetStringAsync(queryUri);

            weather = JsonSerializer.Deserialize<WeatherViewModel>(response);
        }

        async private Task RenewWeather()
        {
            if (weather != null)
            {
                if (DateTime.TryParse(weather.records.location.First().time.obsTime,
                    out DateTime lastUpdate))
                {
                    DateTime now = DateTime.Now;
                    TimeSpan t = now.Subtract(lastUpdate);
                    if (t < new TimeSpan(1, 0, 0))
                    {
                        return;
                    }
                }
            }
            await GetWeather();
        }

        async public Task<string> GetWeatherText()
        {
            await RenewWeather();
            if (weather == null)
            {
                return "";
            }

            return weather.records.location.First().weatherElement.First().elementValue;
        }
    }
}

