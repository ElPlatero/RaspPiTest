using System;
using System.Drawing;

namespace RaspPiTest.Hue.DomainModels
{
    public class LightState
    {
        public bool IsOn { get; set; }
        public bool IsReachable { get; set; }
        public CieCoordinates Coordinates { get; set; }
        public byte Brightness { get; set; }

        public void SetRgbColor(Color color)
        {
            color = Color.FromArgb(255, color.R, color.B, color.G);

            // Normalize.
            var red = (float)color.R / 255;
            var green = (float)color.G / 255;
            var blue = (float)color.B / 255;

            // Apply gamma correction.
            red =   red > 0.04045f   ? (float)Math.Pow((red + 0.055f)   / (1.0f + 0.055f), 2.4f) : red   / 12.92f;
            green = green > 0.04045f ? (float)Math.Pow((green + 0.055f) / (1.0f + 0.055f), 2.4f) : green / 12.92f;
            blue =  blue > 0.04045f  ? (float)Math.Pow((blue + 0.055f)  / (1.0f + 0.055f), 2.4f) : blue  / 12.92f;

            // Convert to xyz (RGB D65 formula, wikipedia.)
            var x = red * 0.649926f  + green * 0.103455f + blue * 0.197109f;
            var y = red * 0.234327f  + green * 0.743075f + blue * 0.022598f;
            var z = red * 0.0000000f + green * 0.053077f + blue * 1.035763f;

            // Calculate xy
            Coordinates = new CieCoordinates(x / (x + y + z), y / (x + y + z));

            // Hue bulbs take care of choosing the closest color they can display

            // set Y of XYZ as Brightness
            Brightness = (byte)(y * byte.MaxValue);
        }
    }


}
