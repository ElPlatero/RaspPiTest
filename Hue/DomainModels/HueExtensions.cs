using HueSharp.Messages.Lights;

namespace RaspPiTest.Hue.DomainModels
{
    public static class HueExtensions
    {
        public static HueSharp.Messages.Lights.Light ToHueType(this Light light)
        {
            return new HueSharp.Messages.Lights.Light
            {
                Id = light.Id,
                Name = light.Name,
                Status = light.State.ToHueType(),
            };
        }

        public static Light ToApiType(this HueSharp.Messages.Lights.Light light)
        {
            return new Light
            {
                Id = light.Id,
                Name = light.Name,
                State = light.Status.ToApiType()
            };
        }

        public static SetLightState ToHueType(this LightState state)
        {
            if(state.IsOn) return new SetLightState
            {
                Brightness = state.Brightness,
                IsOn = state.IsOn,
                Coordinates = new[] {state.Coordinates.X, state.Coordinates.Y},
                IsReachable = state.IsReachable
            };
            return new SetLightState { IsOn = false };
        }

        public static LightState ToApiType(this HueSharp.Messages.Lights.LightState state)
        {
            return new LightState
            {
                IsOn = state.IsOn,
                Coordinates = state.IsOn ? new CieCoordinates(state.Coordinates[0], state.Coordinates[1]) : CieCoordinates.Empty,
                IsReachable = state.IsReachable,
                Brightness = (byte) state.Brightness
            };
        }
    }
}