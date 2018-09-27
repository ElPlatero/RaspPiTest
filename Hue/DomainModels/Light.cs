using System;

namespace RaspPiTest.Hue.DomainModels
{
    public class Light
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LightState State { get; set; }
    }

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

        public static HueSharp.Messages.Lights.LightState ToHueType(this LightState state)
        {
            return new HueSharp.Messages.Lights.LightState
            {
                Brightness = state.Brightness,
                IsOn = state.IsOn,
                Coordinates = new[] {state.Coordinates.X, state.Coordinates.Y},
                IsReachable = state.IsReachable
            };
        }

        public static LightState ToApiType(this HueSharp.Messages.Lights.LightState state)
        {
            return new LightState
            {
                IsOn = state.IsOn,
                Coordinates = new CieCoordinates(state.Coordinates[0], state.Coordinates[1]),
                IsReachable = state.IsReachable,
                Brightness = (byte) state.Brightness
            };
        }
    }
}
