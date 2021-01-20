using Microsoft.AspNetCore.Connections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Communication
{
    public class KestrelClientListenerAdapter
    {
        private readonly ConcurrentQueue<KestrelClientTaskWrapper> _connections = new ConcurrentQueue<KestrelClientTaskWrapper>();

        public Task ExecuteNewConnection(ConnectionContext connection)
        {
            var wrapper = new KestrelClientTaskWrapper(connection);
            _connections.Enqueue(wrapper);
            return wrapper.AwaitConnectionCompletion();
        }

        internal bool HasClientPending() => _connections.Count > 0;

        internal KestrelClientTaskWrapper GetPendingConnectionContext()
        {
            _connections.TryDequeue(out var wrapper);
            return wrapper;
        }
    }
}
