using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Tests.Text
{
    public class TextClientListener : IMailClientListener
    {
        private readonly Stream _stream;
        private bool _pending;

        public TextClientListener(Stream stream)
        {
            _stream = stream;
            _pending = true;
        }

        public bool ClientPending => _pending;

        public IMailClient AcceptClient()
        {
            _pending = false;
            return new TextMailClient(_stream);
        }
    }
}
