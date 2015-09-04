using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMTPileIt.Server
{
    public class EmailStatistics
    {
        private int _emailsReceived = 0;
        private int _errorsGenerated = 0;
        private long _commandsProcessed = 0;
        private long _lastReceivedUtc = DateTime.MinValue.Ticks;
        private int _activeClients = 0;

        public long CommandsProcessed { get { return _commandsProcessed; } }

        public int EmailsReceived { get { return _emailsReceived; } }

        public int ErrorsGenerated { get { return _errorsGenerated; } }

        public DateTime LastMailReceivedUTC { get { return new DateTime(_lastReceivedUtc); } }

        public DateTime LastMailReceivedLocal { get { return LastMailReceivedUTC.ToLocalTime(); } }

        public int ActiveClients { get { return _activeClients; } }

        internal void AddClient(int val)
        {
            Interlocked.Add(ref _activeClients, val);
        }

        internal void RemoveClient(int val)
        {
            Interlocked.Add(ref _activeClients, -1 * val);
        }

        internal void AddEmailReceived()
        {
            Interlocked.Increment(ref _emailsReceived);
            long date = DateTime.UtcNow.Ticks;
            Interlocked.Exchange(ref _lastReceivedUtc, date);
        }

        internal void AddErrorGenerated()
        {
            Interlocked.Increment(ref _errorsGenerated);
        }

        internal void AddCommandProcessed()
        {
            Interlocked.Increment(ref _commandsProcessed);
        }
    }
}
