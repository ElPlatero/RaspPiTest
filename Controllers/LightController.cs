using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueSharp.Messages.Lights;
using Microsoft.AspNetCore.Mvc;
using RaspPiTest.Hue;

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
            return Task.FromResult((IActionResult)Ok(_repository.GetLights()));
        }

        [HttpGet("{lightId}/state")]
        public Task<IActionResult> GetLightState(int lightId)
        {
            var state = _repository.GetState(lightId);
            if (state == null) return Task.FromResult((IActionResult)NotFound());

            return Task.FromResult((IActionResult) Ok(state));

        }

        [HttpPost("{lightId}/state")]
        public Task<IActionResult> SetLightState([FromBody] LightState state, int lightId)
        {
            if (!_repository.SwitchLightState(lightId, state))
            {
                return Task.FromResult((IActionResult) NotFound());
            }

            return Task.FromResult((IActionResult) Ok());
        }
    }
}
