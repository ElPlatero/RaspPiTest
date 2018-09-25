using System;

namespace RaspPiTest.Kachelmann
{
    public class WeatherFetch
    {
        public DateTime Fetched { get; set; }
        public DayForecast Today { get; set; }
        public DayForecast Tommorrow { get; set; }
        public DayForecast DayAfterTommorrow { get; set; }
    }
}