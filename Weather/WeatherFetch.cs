using System;

namespace RaspPiTest.Weather
{
    public class WeatherFetch
    {
        public DateTime Fetched { get; set; }
        public DayForecast Today { get; set; }
        public DayForecast Tommorrow { get; set; }
        public DayForecast DayAfterTommorrow { get; set; }
    }
}