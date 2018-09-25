using System.Xml.Serialization;

namespace RaspPiTest.FritzBox.Model
{
    [XmlRoot(Namespace = "", ElementName = "devicelist")]
    public class DeviceList
    {
        [XmlElement("device")]
        public Device[] Devices { get; set; }
        [XmlAttribute]
        public byte Version { get; set; }
    }
}
