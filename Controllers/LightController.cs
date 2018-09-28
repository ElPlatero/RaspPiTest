using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaspPiTest.Hue;
using RaspPiTest.Hue.DomainModels;

namespace RaspPiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightController : Controller
    {
        private readonly HueRepository _repository;

        public LightController(HueRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<IActionResult> GetAllLights()
        {
            return _repository
                .GetLightsAsync()
                .ContinueWith(getLightsTask => (IActionResult) Ok(getLightsTask.Result));
        }

        [HttpGet("{lightId}/state")]
        public Task<IActionResult> GetLightState(int lightId)
        {
            return _repository
                .GetStateAsync(lightId)
                .ContinueWith(getStateTask =>
                {
                    var state = getStateTask.Result;
                    return state == null ? (IActionResult) NotFound() : Ok(state);
                });
        }

        [HttpPut("{lightId}/state")]
        public Task<IActionResult> SetLightState([FromBody] LightState state, int lightId)
        {
            return _repository
                .SwitchLightStateAsync(lightId, state)
                .ContinueWith(p => p.Result ? (IActionResult) Ok() : NotFound());
        }
    }
}
