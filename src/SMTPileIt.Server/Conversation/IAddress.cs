using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public interface IAddress
    {
        string DisplayName { get; }
        string Address { get; }
        string Host { get; }
        string User { get; }
        AddressType Type { get; }
    }
}
