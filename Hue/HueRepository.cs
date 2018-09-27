using System;
using System.Collections.Generic;
using System.Linq;
using HueSharp.Messages.Lights;
using HueSharp.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace RaspPiTest.Hue
{
    public class HueRepository
    {
        private readonly HueConfiguration _options;
        private ICollection<Light> _knownLights;
        private DateTime _lastFetch;
        private ILogger _logger;

        private ICollection<Light> KnownLights
        {
            get
            {
                Refresh();
                return _knownLights;
            }
        }

        public HueRepository(IOptions<HueConfiguration> options, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<HueRepository>();
        }

        private void Refresh()
        {
            if (_knownLights != null && DateTime.Now - _lastFetch < TimeSpan.FromMinutes(_options.RefreshInterval)) return;

            var client = GetClient();
            var response = client.GetResponse(new GetAllLightsRequest()) as GetAllLightsResponse;
            if (response == null) return;

            _lastFetch = DateTime.Now;
            _knownLights = new HashSet<Light>(response);
        }

        private HueClient GetClient()
        {
            var result = new HueClient(_options.User, _options.Address);
            result.Log += (s, e) => _logger.LogDebug(e);
            return result;
        }

        public IEnumerable<Light> GetLights()
        {
            return KnownLights?? new Light[0];
        }

        public LightState GetState(int lightId)
        {
            var result = KnownLights.SingleOrDefault(p => p.Id == lightId)?.Status;
            if (result != null)
            {
                result.IsOn = result.IsOn;
                result.IsReachable = result.IsReachable;
                result.Brightness = result.Brightness;
                result.Hue = result.Hue;
                result.Saturation = result.Saturation;
                result.Effect = result.Effect;
            }

            return result;
        }

        public bool SwitchLightState(int lightId, LightState newState)
        {
            var knownLight = KnownLights.SingleOrDefault(p => p.Id == lightId);
            if (knownLight == null) return false;

            var client = GetClient();
            var request = new SetLightStateRequest(knownLight.Id);
            if (newState.ShouldSerializeIsOn()) request.Status.IsOn = knownLight.Status.IsOn = newState.IsOn;
            if (newState.ShouldSerializeBrightness()) request.Status.Brightness = knownLight.Status.Brightness = newState.Brightness;
            if (newState.ShouldSerializeHue()) request.Status.Hue = knownLight.Status.Hue = newState.Hue;
            if (newState.ShouldSerializeSaturation()) request.Status.Saturation = knownLight.Status.Saturation = newState.Saturation;
            if (newState.ShouldSerializeEffect()) request.Status.Effect = knownLight.Status.Effect = newState.Effect;
            
            client.GetResponse(request);
            return true;
        }
    }
}
