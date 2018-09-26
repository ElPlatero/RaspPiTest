using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RaspPiTest.Weather
{
    public partial class WeatherRepository
    {
        private const string WEATHER_API = @"https://kachelmannwetter.com/de/ajax_pub/weathernexthoursdays?city_id=";
        private WeatherFetch _currentFetch = new WeatherFetch { Fetched = DateTime.MinValue };
        private readonly WeatherOptions _options;

        public WeatherRepository(IOptions<WeatherOptions> options)
        {
            _options = options.Value;
        }

        public async Task<WeatherFetch> FetchForecastAsync()
        {
            if ((DateTime.Now - _currentFetch.Fetched) <= TimeSpan.FromMinutes(_options.RefreshInterval)) return _currentFetch;
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync($"{WEATHER_API}{_options.CityId}"))
            using (var reader = new StreamReader(stream))
            {
                string line;
                var sb = new StringBuilder("<root>");
                var beginFound = false;
                int divCount = 0;


                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!beginFound && !Regex.IsMatch(line, @"^\s*<div class=""col-xs-4\s*nextdays"">\s*$")) continue;
                    if (!beginFound) beginFound = true;

                    if (line.Contains("<div ")) divCount++;
                    if (line.Contains("</div")) divCount--;

                    if (divCount < 0) break;
                    sb.AppendLine(line);
                }

                sb.AppendLine("</root>");
                var xDoc = XDocument.Parse(sb.ToString());
                _currentFetch = ParseFetch(xDoc);
            }

            return _currentFetch;
        }

        public async Task<WeatherConditions> FetchWeatherConditionsAsync()
        {
            var yql = $"select item.condition from weather.forecast where u = 'c' and woeid in (select woeid from geo.places where text = '{_options.CityName}')";
            var uri = $"https://query.yahooapis.com/v1/public/yql?q={Uri.EscapeDataString(yql)}&format=json";

            JObject jObject;

            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(uri))
            using (var reader = new StreamReader(stream))
            {
                jObject = JObject.Parse(reader.ReadToEnd());
            }

            var condition = jObject["query"]["results"]["channel"]["item"]["condition"];
            if (condition == null) throw new InvalidOperationException($"unexpected response: {jObject}");

            var dateString = condition["date"].ToString().Replace("CET", "+01:00").Replace("CEST", "+02:00");

            var t = DateTime.ParseExact(dateString, "ddd, dd MMM yyyy HH':'mm tt zzz", CultureInfo.InvariantCulture);

            return new WeatherConditions(
                condition["code"].Value<int>(), 
                t, 
                condition["temp"].Value<float>()
            );
        }

        private WeatherFetch ParseFetch(XDocument xDoc)
        {
            if (xDoc?.Root == null) throw new ArgumentNullException(nameof(xDoc));

            var days = xDoc.Root.Descendants("div").Where(p => p.Attribute("class")?.Value == "col-xs-4  nextdays").ToArray();
            var result = new WeatherFetch
            {
                Today = ParseDay(days[0]),
                Tommorrow = ParseDay(days[1]),
                DayAfterTommorrow = ParseDay(days[2]),
                Fetched = DateTime.Now
            };
            return result;
        }
        private DayForecast ParseDay(XElement element)
        {
            var result = new DayForecast();

            var date = DateTime.Parse(Regex.Replace(element.Elements().First().Elements().First().Value, @".*\n", "").Trim());
            if (date < DateTime.Now && date.DayOfYear != DateTime.Now.DayOfYear) date = new DateTime(date.Year + 1, date.Month, date.Day);
            result.Date = date;

            var sunshineInfos = element.Descendants("img").Where(p => p.Parent?.Attribute("title") != null && p.Parent.Attribute("title").Value.Contains(":")).Select(p =>
            {
                var node = p.Parent.Attribute("title").Value.Split(':');
                return new { TimeOfDay = node[0], Sunshine = GetSunshine(node[1].Trim()) };
            }).ToList();
            result.Morning = sunshineInfos.First(p => p.TimeOfDay == "vormittags").Sunshine;
            result.Afternoon = sunshineInfos.First(p => p.TimeOfDay == "nachmittags").Sunshine;
            result.Night = sunshineInfos.First(p => p.TimeOfDay == "abends").Sunshine;

            result.MinTemperature = float.Parse(element
                .Descendants("div")
                .First(p =>
                    p.Attribute("class") != null &&
                    p.Attribute("class").Value == "day-fc-temp" &&
                    p.ElementsAfterSelf().First().Value == "min").Value.TrimEnd('°'));
            result.MaxTemperature = float.Parse(element
                .Descendants("div")
                .First(p =>
                    p.Attribute("class") != null &&
                    p.Attribute("class").Value == "day-fc-temp" &&
                    p.ElementsAfterSelf().First().Value == "max").Value.TrimEnd('°'));

            result.ExpectedRain = float.Parse(element.Descendants("span")
                .First(p => p.Attribute("class") != null && p.Attribute("class").Value.Contains("wi-umbrella")).Parent.Value
                .TrimEnd('m'));

            result.Wind = GetWind(element.Descendants("div").First(p =>
                p.Attribute("class") != null && p.Attribute("class").Value == "day-wind-rain"));

            return result;
        }
        private Wind GetWind(XElement element)
        {
            var result = new Wind();

            var directionNode = element.Descendants("span").First(p => p.Attribute("class") != null && p.Attribute("class").Value.Contains("wi-direction"));
            result.Direction = GetWindDirection(directionNode.Attribute("class").Value.Split('-').Last());

            var nodes = element.Elements().ToArray();
            result.Speed = float.Parse(nodes[0].Value.Substring(0, nodes[0].Value.Length - 4));
            result.MaxSpeed = float.Parse(nodes[1].Value.Substring(0, nodes[1].Value.Length - 4));
            return result;
        }
        private Sunshine GetSunshine(string sunhineDescription)
        {
            switch (sunhineDescription)
            {
                case "wolkenlos": return Sunshine.Wolkenlos;
                case "wolkig, stark bewölkt": return Sunshine.StarkBewoelkt;
                case "bedeckt": return Sunshine.Bedeckt;
                case "Regen": return Sunshine.Regen;
                case "Gewitter möglich": return Sunshine.Gewitter;
                case "leicht bewölkt": return Sunshine.LeichtBewoelkt;
                default: return Sunshine.Wolkenlos;
            }
        }
        private WindDirection GetWindDirection(string value)
        {
            switch (value)
            {
                case "n": return WindDirection.North;
                case "nne": return WindDirection.NortNorthEast;
                case "ne": return WindDirection.NorthEast;
                case "ene": return WindDirection.EastNorthEast;
                case "e": return WindDirection.East;
                case "ese": return WindDirection.EastSouthEast;
                case "se": return WindDirection.SouthEast;
                case "sse": return WindDirection.SouthSouthEast;
                case "s": return WindDirection.South;
                case "ssw": return WindDirection.SouthSouthWest;
                case "sw": return WindDirection.SouthWest;
                case "wsw": return WindDirection.WestSouthWest;
                case "w": return WindDirection.West;
                case "wnw": return WindDirection.WestNorthWest;
                case "nw": return WindDirection.NorthWest;
                case "nnw": return WindDirection.NorthNorthWest;
                default: return WindDirection.None;

            }
        }
    }
}
