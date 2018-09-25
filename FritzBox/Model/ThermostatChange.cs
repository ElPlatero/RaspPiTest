using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RaspPiTest.FritzBox.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ThermostatChange
    {
        [XmlElement("endperiod")]
        public uint EndperiodInternal { get; set; }
        [XmlElement("tchange")]
        public byte TchangeInternal { get; set; }

        [JsonProperty("changeTime")]
        public DateTime ChangeTime => EndperiodInternal == 0
            ? DateTime.MinValue
            : new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(EndperiodInternal);

        [JsonProperty("newTemperature")]
        public float NewTemperatur => TchangeInternal == 0 || TchangeInternal == 255 ? 0f : TchangeInternal / 2.0f;
    }
}