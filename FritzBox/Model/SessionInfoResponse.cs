using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RaspPiTest.FritzBox.Model
{
    public class SessionInfoResponse : IXmlSerializable
    {
        public string SessionId { get; set; }
        public string Challenge { get; set; }
        public int BlockTime { get; set; }
        public ICollection<SessionInfoRights> Rights { get; } = new HashSet<SessionInfoRights>();
        public bool IsSessionIdSet => SessionId != "0000000000000000";

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element) continue;
                if (reader.Name == "SID") SessionId = reader.ReadElementContentAsString();
                if (reader.Name == "Challenge") Challenge = reader.ReadElementContentAsString();
                if (reader.Name == "BlockTime") BlockTime = reader.ReadElementContentAsInt();
                if (reader.Name == "Name")
                {
                    var rightsName = reader.ReadElementContentAsString();
                    while (reader.Name != "Access") reader.Read();
                    Rights.Add(new SessionInfoRights { Name = rightsName, Access = (byte)reader.ReadElementContentAsInt()});
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
