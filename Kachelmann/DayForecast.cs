using System;

namespace RaspPiTest.Kachelmann
{
    public class DayForecast
    {
        public DateTime Date { get; set; }
        public Sunshine Morning { get; set; }
        public Sunshine Afternoon { get; set; }
        public Sunshine Night { get; set; }
        public float MaxTemperature { get; set; }
        public float MinTemperature { get; set; }
        public Wind Wind { get; set; }
        public float ExpectedRain { get; set; }
    }
}