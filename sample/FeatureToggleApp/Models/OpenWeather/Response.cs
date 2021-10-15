namespace FeatureToggleApp.Models.OpenWeather
{
    public class Response
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List[] list { get; set; }
        public City city { get; set; }
    }
}
