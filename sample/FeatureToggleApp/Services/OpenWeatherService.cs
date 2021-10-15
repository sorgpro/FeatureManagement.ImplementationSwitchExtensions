using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FeatureToggleApp.Configurations;
using FeatureToggleApp.Models;
using FeatureToggleApp.Models.OpenWeather;
using Microsoft.Extensions.Options;

namespace FeatureToggleApp.Services
{
    public class OpenWeatherService : IWeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _apiKey;

        public OpenWeatherService(
            HttpClient httpClient,
            IOptions<OpenWeatherConfiguration> options,
            IMapper mapper)
        {
            _httpClient = httpClient; // Настроенный HTTP-клиент предоставляется фабрикой.
            _mapper = mapper;
            _apiKey = options.Value.ApiKey; // Получение API-ключа из конфигурации.
        }

        public Task<IEnumerable<WeatherForecastResponse>> GetByIdAsync(string cityId)
        {
            string path = $"forecast?id={cityId}&appid={_apiKey}&units=metric&cnt=5&lang=ru";
            return GetByPathAsync(path);
        }

        public Task<IEnumerable<WeatherForecastResponse>> GetByNameAsync(string cityName)
        {
            string path = $"forecast?q={cityName}&appid={_apiKey}&units=metric&cnt=5&lang=ru";
            return GetByPathAsync(path);
        }

        private async Task<IEnumerable<WeatherForecastResponse>> GetByPathAsync(string path)
        {
            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(path);
            httpResponseMessage.EnsureSuccessStatusCode();
            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            using Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var weatherResponse = await JsonSerializer.DeserializeAsync<Response>(stream);
            var result = weatherResponse.list.Select(x => _mapper.Map<WeatherForecastResponse>(x));
            return result;
        }
    }
}
