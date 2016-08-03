using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Data
{
    [CollectionDataContract]
    internal class XmlAddressList : Collection<string>
    {
        protected XmlAddressList() { }

        internal XmlAddressList(IList<string> elements)
            : base(elements)
        {

        }
    }
}
