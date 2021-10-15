namespace FeatureToggleApp.Configurations
{
    /// <summary>
    /// Конфигурация взаимодействия с сервисом прогноза погоды OpenWeather.
    /// </summary>
    public class OpenWeatherConfiguration
    {
        /// <summary>
        /// Базовый адрес сервиса.
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Идентификатор города, для которого запрашивается прогноз.
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// API-ключ, доступный после регистрации по адресу https://home.openweathermap.org/api_keys.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
