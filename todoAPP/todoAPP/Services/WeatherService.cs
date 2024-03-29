﻿using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using todoAPP.ViewModel;

namespace todoAPP.Services
{
    public class WeatherService
    {
        private Uri BaseURI;

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public WeatherService(IHttpClientFactory clientFactory, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;

            BaseURI = new Uri("https://opendata.cwa.gov.tw");
        }

        async private Task<WeatherViewModel?> GetWeather()
        {
            try
            {
                Uri apiUri = new Uri(BaseURI, "/api/v1/rest/datastore/O-A0001-001");
                QueryBuilder qBuilder = new QueryBuilder
                {
                    { "Authorization", _configuration.GetSection("CWB:Key").Value },
                    { "StationId", "C0AC70" },
                    { "WeatherElement", "Weather" }
                };
                string queryParamStr = qBuilder.ToQueryString().ToUriComponent();

                Uri queryUri = new Uri(apiUri, queryParamStr);
                var httpClient = _clientFactory.CreateClient();
                var response = await httpClient.GetStringAsync(queryUri);

                return JsonSerializer.Deserialize<WeatherViewModel>(response);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        async private Task<WeatherViewModel?> RenewWeather()
        {
            WeatherViewModel? weather;
            if (_memoryCache.TryGetValue("weather", out weather) == false)
            {
                weather = await GetWeather();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set("weather", weather, cacheEntryOptions);
            }
            return weather;
        }

        async public Task<string?> GetWeatherText()
        {
            WeatherViewModel? weather = await RenewWeather();

            return weather?.records.Station?.First().WeatherElement.Weather;
        }
    }
}

