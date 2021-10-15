using System;
using AutoMapper;
using FeatureToggleApp.Models;
using FeatureToggleApp.Models.OpenWeather;

namespace FeatureToggleApp.Configurations
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<List, WeatherForecastResponse>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.dt_txt)))
                .ForMember(dest => dest.TemperatureC, opt => opt.MapFrom(src => src.main.temp))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.weather[0].description));
        }
    }
}
