using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaspPiTest.FritzBox;
using RaspPiTest.FritzBox.Model;
using RaspPiTest.Middleware;

namespace RaspPiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly FritzBoxClient _client;
        private readonly ILogger _logger;

        public DeviceController(ILoggerFactory loggerFactory, FritzBoxClient client)
        {
            _logger = loggerFactory.CreateLogger<DeviceController>();
            _client = client;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (_logger.BeginScope("Get Devices"))
            {
                _logger.LogDebug("Ermittlung Geräteliste...");
                var result = await _client.ReadPageAsync<DeviceList>("http://fritz.box/webservices/homeautoswitch.lua?switchcmd=getdevicelistinfos");
                if (result == null || !result.Devices.Any())
                {
                    throw new ApiException(StatusCodes.Status404NotFound, "Fehlerhaft eingelesen, oder keine Geräte verfügbar.");
                }

                _logger.LogInformation("Geräteliste erfolgreich abgerufen: {@list}",  result.Devices.Select(p => new {p.Id, p.Name}));
                return Ok(new {result.Devices});
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> Put(string id, [FromBody] float newTemperature)
        {
            using (_logger.BeginScope("Update Device"))
            {
                _logger.LogDebug("Aktualisiere Gerät {id}...", id);

                var parameter = Thermostat.GetFritzboxTemperature(newTemperature);
                var result = await _client.ReadPageAsync<string>($"http://fritz.box/webservices/homeautoswitch.lua?ain={id}&switchcmd=sethkrtsoll&param={parameter}");

                return !byte.TryParse(result.Trim('\n'), out byte setTemperature)
                    ? Ok(new {success = false})
                    : Ok(new {success = true, result = Thermostat.GetTemperature(setTemperature)});
            }
        }
    }
}
