using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaspPiTest.Weather;

namespace RaspPiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherRepository _weatherRepository;
        private readonly ILogger _logger;

        public WeatherController(ILoggerFactory loggerFactory, WeatherRepository weatherRepository)
        {
            _logger = loggerFactory.CreateLogger<WeatherController>();
            _weatherRepository = weatherRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherConditions()
        {
            using (_logger.BeginScope("Get conditions"))
            {
                return Ok(await _weatherRepository.FetchWeatherConditionsAsync());
            }
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetThreeDaysForecast()
        {
            using (_logger.BeginScope("Get forecast"))
            {
                return Ok(await _weatherRepository.FetchForecastAsync());
            }
        }
    }
}
