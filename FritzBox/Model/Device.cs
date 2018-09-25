using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RaspPiTest.FritzBox.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Device
    {
        [XmlElement("present")]
        public byte IsPresent { get; set; }

        [XmlElement("name"), JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("temperature"), JsonProperty("deviceTemperature")]
        public DeviceTemperature Temperature { get; set; }

        [XmlElement("hkr"), JsonProperty("thermostat")]
        public Thermostat Thermostat { get; set; }

        [XmlAttribute("identifier"), JsonProperty("ain")]
        public string Identifier { get; set; }

        [XmlAttribute("id")]
        public byte Id { get; set; }

        [XmlAttribute("functionbitmask")]
        public ushort Functions { get; set; }

        [XmlAttribute("fwversion")]
        public decimal FwVersion { get; set; }

        [XmlAttribute("manufacturer"), JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [XmlAttribute("productname"), JsonProperty("productName")]
        public string ProductName { get; set; }
    }
}