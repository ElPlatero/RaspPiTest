using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RaspPiTest.FritzBox.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceTemperature
    {
        [XmlElement("celsius")]
        public int CelsiusInternal { get; set; }
        [JsonProperty("temperature")] public float Temperature => CelsiusInternal / 10f;

        [XmlElement("offset")]
        public int OffsetInternal { get; set; }
        [JsonProperty("offset")] public float Offset => OffsetInternal / 10f;
    }
}