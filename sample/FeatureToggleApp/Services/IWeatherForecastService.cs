using System.Collections.Generic;
using System.Threading.Tasks;
using FeatureToggleApp.Models;

namespace FeatureToggleApp.Services
{
    public interface IWeatherForecastService
    {
        public Task<IEnumerable<WeatherForecastResponse>> GetByIdAsync(string cityId);
        public Task<IEnumerable<WeatherForecastResponse>> GetByNameAsync(string cityName);
    }
}
