using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Azure;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public class WeatherService
    {
        private Uri BaseURI;
        private string API;
        private IEnumerable<KeyValuePair<string, string>> Params;

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public WeatherService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;

            BaseURI = new Uri("https://opendata.cwb.gov.tw");
        }

        async public Task<WeatherViewModel?> GetWeather()
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

            WeatherViewModel? weather = JsonSerializer.Deserialize<WeatherViewModel>(response);

            return weather;
        }
    }
}

