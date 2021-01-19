using SmtpPilot.Server.IO;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server.Communication
{
    public class KestrelClientListener : IMailClientListener
    {
        private readonly KestrelClientListenerAdapter _adapter;

        public KestrelClientListener(KestrelClientListenerAdapter adapter)
        {
            _adapter = adapter;
        }

        public bool ClientPending => _adapter.HasClientPending();

        public IMailClient AcceptClient()
        {
            var wrapper = _adapter.GetPendingConnectionContext();
            return new KestrelMailClient(wrapper.Context);
        }
    }
}
