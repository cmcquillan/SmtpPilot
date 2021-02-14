using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SmtpPilot.Server.Data
{
    [CollectionDataContract(Namespace = XmlMailMessage.Namespace)]
    internal class XmlAddressList : Collection<string>
    {
        protected XmlAddressList() { }

        internal XmlAddressList(IList<string> elements)
            : base(elements)
        {

        }
    }
}
