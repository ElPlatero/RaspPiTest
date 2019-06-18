using System;
using System.Linq;

namespace RaspPiTest.Weather
{
    public class OpenWeatherMapApiKey
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class OpenWeatherMapConfiguration
    {
        public OpenWeatherMapApiKey[] OwmKeys { get; set; }

        public string Get(string name) => OwmKeys.SingleOrDefault(p => p.Name.Equals(name))?.Value 
                                          ?? throw new InvalidOperationException($@"The owm API key ""{name}"" could not be found.");
    }
}
