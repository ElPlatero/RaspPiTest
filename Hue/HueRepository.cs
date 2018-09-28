using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueLight = HueSharp.Messages.Lights.Light;
using HueLightState = HueSharp.Messages.Lights.LightState;
using HueSharp.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspPiTest.Hue.DomainModels;
using Light = RaspPiTest.Hue.DomainModels.Light;
using LightState = RaspPiTest.Hue.DomainModels.LightState;


namespace RaspPiTest.Hue
{
    public class HueRepository
    {
        private ICollection<Light> _knownLights = new Light[0];
        private DateTime _lastFetch;
        private readonly HueClient _hueClient;
        private ILogger _logger;
        private readonly TimeSpan _refreshIntervall;

        public HueRepository(IOptions<HueConfiguration> options, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HueRepository>();
            _hueClient = new HueClient(loggerFactory, options.Value.User, options.Value.Address);
            _refreshIntervall = TimeSpan.FromMinutes(options.Value.RefreshInterval);
        }

        private Task<T> WrapAsync<T>(Func<ICollection<Light>, T> innerFunction)
        {
            return GetKnownLightsAsync().ContinueWith(getKnownLightsTask =>
            {
                var lights = getKnownLightsTask.Result;
                return innerFunction(lights);
            });
        }

        private async Task<ICollection<Light>> GetKnownLightsAsync()
        {
            if (DateTime.Now - _lastFetch < _refreshIntervall) return _knownLights;

            var response = await _hueClient.GetResponseAsync(new GetAllLightsRequest()) as GetAllLightsResponse;
            if (response == null) return _knownLights;

            _lastFetch = DateTime.Now;
            _knownLights = response.Select(HueExtensions.ToApiType).ToArray();
            return _knownLights;
        }

        public Task<Light[]> GetLightsAsync()
        {
            return WrapAsync(p => p.ToArray());
        }

        public Task<LightState> GetStateAsync(int lightId)
        {
            return WrapAsync(lights => lights.SingleOrDefault(p => p.Id == lightId)?.State);
        }

        public Task<bool> SwitchLightStateAsync(int lightId, LightState newState)
        {
            return WrapAsync(lights => 
            {
                var knownLight = lights.SingleOrDefault(p => p.Id == lightId);
                if (knownLight == null) return false;

                var request = new SetLightStateRequest(knownLight.Id) {Status = newState.ToHueType()};
                var response = _hueClient.GetResponseAsync(request).Result;

                if (response is SuccessResponse)
                {
                    knownLight.State = newState;
                    return true;
                }
                return false;
            });
        }
    }
}
