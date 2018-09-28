namespace RaspPiTest.Hue.DomainModels
{
    public sealed class CieCoordinates
    {
        public static readonly CieCoordinates Empty = new CieCoordinates(0,0);
        public static readonly CieCoordinates Candle = new CieCoordinates(0.5268, 0.4133); //actual candle is about 500K warmer than this.
        public static readonly CieCoordinates SodiumVaporLamp = new CieCoordinates(0.5268, 0.4133);
        public static readonly CieCoordinates Bulb40W = new CieCoordinates(0.4677, 0.4121);
        public static readonly CieCoordinates Bulb60W = new CieCoordinates(0.4596, 0.4107);
        public static readonly CieCoordinates Bulb100W = new CieCoordinates(0.4511, 0.4084);
        public static readonly CieCoordinates Bulb200W = new CieCoordinates(0.4364, 0.4039);
        public static readonly CieCoordinates PhotoBulbB = new CieCoordinates(0.4244, 0.3993);
        public static readonly CieCoordinates LateEvening = new CieCoordinates(0.4244, 0.3993);
        public static readonly CieCoordinates OrLighting = new CieCoordinates(0.3984, 0.3871);
        public static readonly CieCoordinates FluorescentLamp = new CieCoordinates(0.3804, 0.3768);
        public static readonly CieCoordinates MoonLight = new CieCoordinates(0.3748, 0.3732);
        public static readonly CieCoordinates XenonLight = new CieCoordinates(0.3522, 0.3572);
        public static readonly CieCoordinates EarlyMorningSun = new CieCoordinates(0.3453, 0.3517);
        public static readonly CieCoordinates LateMorningSun = new CieCoordinates(0.3312, 0.34);
        public static readonly CieCoordinates CloudyNoon = new CieCoordinates(0.3285, 0.3376);
        public static readonly CieCoordinates CloudedNoon = new CieCoordinates(0.3124, 0.3226);

        private double _x;
        private double _y;

        public CieCoordinates(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X
        {
            get => _x;
            set => _x = value < 0 ? 0 : value > 1 ? 1 : value;
        }

        public double Y
        {
            get => _y;
            set => _y = value < 0 ? 0 : value > 1 ? 1 : value;
        }
    }


}
