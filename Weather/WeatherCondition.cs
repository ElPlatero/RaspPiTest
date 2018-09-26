using System;

namespace RaspPiTest.Weather
{
    public partial class WeatherRepository
    {
        public class WeatherConditions
        {
            public WeatherConditions(int code, DateTime fetchDate, float temperature)
            {
                Sunshine = ConvertYahooConditionCode(code);
                FetchDate = fetchDate;
                Temperature = temperature;
            }

            public Sunshine Sunshine { get; }
            public DateTime FetchDate { get; }
            public float Temperature { get; }

            private static Sunshine ConvertYahooConditionCode(int yahooConditionCode)
            {
                if (yahooConditionCode >= 3 && yahooConditionCode <= 4) return Sunshine.Gewitter;
                if (yahooConditionCode >= 5 && yahooConditionCode <= 18) return Sunshine.Regen;
                if (yahooConditionCode >= 32 && yahooConditionCode <= 36) return Sunshine.Wolkenlos;
                if (yahooConditionCode >= 29 && yahooConditionCode <= 30) return Sunshine.LeichtBewoelkt;
                if (yahooConditionCode >= 27 && yahooConditionCode <= 28) return Sunshine.StarkBewoelkt;
                return Sunshine.Bedeckt;
            }
        }
    }
}
