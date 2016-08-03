using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Data
{
    [DataContract]
    internal class XmlMailHeader
    {
        [DataMember]
        internal string Name { get; set; }

        [DataMember]
        internal string Value { get; set; }
    }
}
