using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FeatureToggleApp.Configurations;
using FeatureToggleApp.Models;
using FeatureToggleApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureToggleApp.Controllers
{
    [Authorize(Policy = "Something")]
    [Route("weather-forecast")]
    public class WeatherForecastController
    {
        [HttpGet]
        public async Task<IEnumerable<WeatherForecastResponse>> Get(
            [FromServices] IWeatherForecastService weatherForecastService,
            [FromServices] IOptions<OpenWeatherConfiguration> options)
        {
            var result = await weatherForecastService.GetByIdAsync(options.Value.CityId);
            return result;
        }

        [FeatureGate(FeatureFlags.UseOpenWeatherByCityName)]
        [HttpGet("by-name")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecastResponse>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IEnumerable<WeatherForecastResponse>> GetByName(
            [Required] string cityName,
            [FromServices] IWeatherForecastService weatherForecastService)
        {
            var result = await weatherForecastService.GetByNameAsync(cityName);
            return result;
        }
    }
}
