using System;
using System.Net.Http.Json;
using System.Text.Json;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public class WeatherService
    {
        private string BaseURL = "https://opendata.cwb.gov.tw/api";
        private string API;
        private Dictionary<string, string> Params;

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public WeatherService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        async public Task<WeatherViewModel?> GetWeather()
        {
            API = "/v1/rest/datastore/O-A0001-001";

            Params = new Dictionary<string, string>() { };
            Params.Add("Authorization", _configuration.GetSection("CWB:Key").Value);
            Params.Add("stationId", "C0AC70");
            Params.Add("elementName", "Weather");

            var httpClient = _clientFactory.CreateClient();
            string paramString = "";
            int counter = 0;
            foreach (KeyValuePair<string, string> pair in Params)
            {
                paramString += pair.Key + "=" + pair.Value;
                if (++counter < Params.Count)
                {
                    paramString += "&";
                }
            }

            var response = await httpClient.GetStringAsync(BaseURL + API + "?" + paramString);
            WeatherViewModel? weather = JsonSerializer.Deserialize<WeatherViewModel>(response);

            return weather;
        }
    }
}

