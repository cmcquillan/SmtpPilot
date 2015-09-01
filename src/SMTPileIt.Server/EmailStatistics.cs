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

        public int EmailsReceived { get { return _emailsReceived; } }

        internal void AddEmailReceived()
        {
            Interlocked.Increment(ref _emailsReceived);
        }
    }
}
