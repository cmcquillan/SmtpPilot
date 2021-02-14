using System.Runtime.Serialization;

namespace SmtpPilot.Server.Data
{
    [DataContract(Namespace = XmlMailMessage.Namespace)]
    internal class XmlMailHeader
    {
        [DataMember]
        internal string Name { get; set; }

        [DataMember]
        internal string Value { get; set; }
    }
}
