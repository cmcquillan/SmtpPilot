using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.IO
{
    public interface IMailClient
    {
        int ClientId { get; }

        void Write(string message);

        string Read();

        void Disconnect();

        bool Disconnected { get; }
    }
}
