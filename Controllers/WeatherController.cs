using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaspPiTest.Weather;

namespace RaspPiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherRepository _weatherRepository;

        public WeatherController(WeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherConditions()
        {
            return Ok(await _weatherRepository.FetchWeatherConditionsAsync());
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetThreeDaysForecast()
        {
            return Ok(await _weatherRepository.FetchForecastAsync());
        }
    }
}
