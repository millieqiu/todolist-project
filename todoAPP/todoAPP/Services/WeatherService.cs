using System;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;

namespace todoAPP.Services
{
	public class WeatherService
	{
		private string BaseURL = "https://opendata.cwb.gov.tw/api";
        private string API = "/v1/rest/datastore/O-A0001-001";
		private Dictionary<string,string> Params;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public WeatherService(IHttpClientFactory clientFactory, IConfiguration configuration)
		{
            _clientFactory = clientFactory;
            _configuration = configuration;

            Params = new Dictionary<string, string>() { };
			Params.Add("Authorization", _configuration.GetSection("CWB:Key").Value);
            Params.Add("stationId", "C0AC70");
			Params.Add("elementName", "Weather");

        }

		async public Task<string> GetWeather()
		{
            var httpClient = _clientFactory.CreateClient();
            string paramString = "";
            int counter = 0;
            foreach (KeyValuePair<string,string> pair in Params)
            {
                paramString += pair.Key + "=" + pair.Value;
                if(++counter < Params.Count)
                {
                    paramString += "&";
                }
            }

            string response = await httpClient.GetStringAsync(BaseURL + API + "?" + paramString);
            JObject jResponse = JObject.Parse(response);
            var jRecords = jResponse["records"]["location"][0]["weatherElement"][0]["elementValue"];
            return jRecords.ToString();
        }
    }
}

