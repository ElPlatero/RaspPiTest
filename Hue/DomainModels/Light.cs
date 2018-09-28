namespace RaspPiTest.Hue.DomainModels
{
    public class Light
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LightState State { get; set; }
    }
}
