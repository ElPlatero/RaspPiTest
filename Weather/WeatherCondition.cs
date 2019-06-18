using System;

namespace RaspPiTest.Weather
{
    public partial class WeatherRepository
    {
        public class WeatherConditions
        {
            public WeatherConditions(int code, DateTime fetchDate, float temperature)
            {
                Sunshine = ConvertOwmConditionCode(code);
                FetchDate = fetchDate;
                Temperature = temperature;
            }

            public Sunshine Sunshine { get; }
            public DateTime FetchDate { get; }
            public float Temperature { get; }

            private static Sunshine ConvertOwmConditionCode(int yahooConditionCode)
            {
                if (yahooConditionCode >= 200 && yahooConditionCode <= 232) return Sunshine.Gewitter;
                if (yahooConditionCode >= 300 && yahooConditionCode <= 531) return Sunshine.Regen;
                if (yahooConditionCode == 800) return Sunshine.Wolkenlos;
                if (yahooConditionCode >= 801 && yahooConditionCode <= 802) return Sunshine.LeichtBewoelkt;
                if (yahooConditionCode >= 802 && yahooConditionCode <= 803) return Sunshine.StarkBewoelkt;
                if (yahooConditionCode == 804) return Sunshine.Bedeckt;
                return Sunshine.LeichtBewoelkt;
            }
        }
    }
}
