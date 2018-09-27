namespace RaspPiTest.Hue
{
    public class HueConfiguration
    {
        public string User { get; set; } = string.Empty;
        public string Address { get; set; } = "localhost";
        public int RefreshInterval { get; set; } = 120;
    }
}