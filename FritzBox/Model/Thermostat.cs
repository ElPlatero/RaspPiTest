using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RaspPiTest.FritzBox.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Thermostat
    {
        [XmlElement("tist")]
        public byte ActualTemperatureInternal { get; set; }
        [XmlElement("tsoll")]
        public byte NominalTemperatureInternal { get; set; }
        [XmlElement("absenk")]
        public byte SavingTemperatureInternal { get; set; }
        [XmlElement("komfort")]
        public byte ComfortTemperatureInternal { get; set; }
        [XmlElement("lock")]
        public byte? IsUiLockedInternal { get; set; }
        [XmlElement("devicelock")]
        public byte? IsDeviceLockedInternal { get; set; }
        [XmlElement("errorcode")]
        public byte ErrorCodeInternal { get; set; }
        [XmlElement("batterylow")]
        public byte IsBatteryLowInternal { get; set; }
        [XmlElement("nextchange")]
        [JsonProperty("nextChange")]
        public ThermostatChange NextChange { get; set; }

        public static float GetTemperature(byte internalValue)
        {
            if (internalValue == 253) return -273.15f;
            if (internalValue == 254) return 0;
            return internalValue / 2f;
        }

        public static byte GetFritzboxTemperature(float celsiusTemperature)
        {
            if (celsiusTemperature < 0) return 253;
            if (Math.Abs(celsiusTemperature) < 0.1) return 254;
            return Convert.ToByte(2 * celsiusTemperature);


        }

        [JsonProperty("actualTemperature")]  public float ActualTemperature => GetTemperature(ActualTemperatureInternal);
        [JsonProperty("nominalTemperature")] public float NominalTemperature => GetTemperature(NominalTemperatureInternal);
        [JsonProperty("savingTemperature")]  public float SavingTemperature => GetTemperature(SavingTemperatureInternal);
        [JsonProperty("comfortTemperature")] public float ComfortTemperature => GetTemperature(ComfortTemperatureInternal);
        [JsonProperty("isUiLocked")]         public bool IsUiLocked => IsUiLockedInternal == 1;
        [JsonProperty("isDeviceLocked")]     public bool IsDeviceLocked => IsDeviceLockedInternal == 1;
        [JsonProperty("errorCode")]          public ThermostatErrorCode ErrorCode => (ThermostatErrorCode) ErrorCodeInternal;
        [JsonProperty("isBatteryLow")]       public bool IsBatteryLow => IsBatteryLowInternal == 1;
    }
}