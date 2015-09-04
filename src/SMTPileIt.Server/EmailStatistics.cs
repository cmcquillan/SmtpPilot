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

        public long CommandsProcessed { get { return _commandsProcessed; } }

        public int EmailsReceived { get { return _emailsReceived; } }

        public int ErrorsGenerated { get { return _errorsGenerated; } }

        internal void AddEmailReceived()
        {
            Interlocked.Increment(ref _emailsReceived);
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
