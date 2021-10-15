namespace FeatureToggleApp.Configurations
{
    public static class FeatureFlags
    {
        /// <summary>
        /// Использовать заглушку для внешней зависимости OpenWeather.
        /// </summary>
        public const string UseOpenWeatherStub = nameof(UseOpenWeatherStub);

        /// <summary>
        /// Использовать конечную точку 'by-name'.
        /// </summary>
        public const string UseOpenWeatherByCityName = nameof(UseOpenWeatherByCityName);

        /// <summary>
        /// При запуске сервиса: подавлять авторизацию.
        /// </summary>
        public const string OnStart_SuppressAuth = nameof(OnStart_SuppressAuth);
    }
}
