using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.IO
{
    public interface IMailClientListener
    {
        bool ClientPending { get; }
        IMailClient AcceptClient();
    }
}
