using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaspPiTest.FritzBox;
using RaspPiTest.FritzBox.Model;

namespace RaspPiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly FritzBoxClient _client;

        public DeviceController(FritzBoxClient client)
        {
            _client = client;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _client.ReadPageAsync<DeviceList>("http://fritz.box/webservices/homeautoswitch.lua?switchcmd=getdevicelistinfos");
            if (result == null || !result.Devices.Any()) return NotFound(new { Message = "Fehlerhaft eingelesen, oder keine Geräte verfügbar."});

            return Ok(new { result.Devices});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> Put(string id, [FromBody] float newTemperature)
        {
            var parameter = Thermostat.GetFritzboxTemperature(newTemperature);
            var result = await _client.ReadPageAsync<string>($"http://fritz.box/webservices/homeautoswitch.lua?ain={id}&switchcmd=sethkrtsoll&param={parameter}");

            return !byte.TryParse(result.Trim('\n'), out byte setTemperature) ? Ok(new { success = false}) : Ok(new { success = true, result = Thermostat.GetTemperature(setTemperature) });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
