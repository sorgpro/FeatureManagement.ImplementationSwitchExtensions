using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeatureToggleApp.Models;

namespace FeatureToggleApp.Services
{
    public class OpenWeatherServiceStub : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<IEnumerable<WeatherForecastResponse>> GetByIdAsync(string cityId)
        {
            return GenerateAsync();
        }

        public Task<IEnumerable<WeatherForecastResponse>> GetByNameAsync(string cityName)
        {
            return GenerateAsync();
        }

        private Task<IEnumerable<WeatherForecastResponse>> GenerateAsync()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecastResponse
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
            return Task.FromResult(result);
        }
    }
}
