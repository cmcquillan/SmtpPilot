using Microsoft.AspNetCore.Connections;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Communication
{
    internal class KestrelClientTaskWrapper
    {
        private readonly ConnectionContext _context;
        private readonly TaskCompletionSource<bool> _tcs;

        internal KestrelClientTaskWrapper(ConnectionContext context)
        {
            _context = context;
            _tcs = new TaskCompletionSource<bool>();
        }

        internal Task AwaitConnectionCompletion()
        {
            return _tcs.Task;
        }

        internal void SetConnectionCompleted(bool result)
        {
            _tcs.SetResult(result);
        }

        internal ConnectionContext Context => _context;
    }
}
