using System;
using System.Threading;

namespace SmtpPilot.Server
{
    public class EmailStatistics
    {
        #region Tracking Fields
        private int _emailsReceived = 0;
        private int _errorsGenerated = 0;
        private long _commandsProcessed = 0;
        private long _lastReceivedUtc = DateTimeOffset.MinValue.Ticks;
        private int _activeClients = 0;
        private long _startTime = 0;
        #endregion

        #region Tracking Methods
        internal void SetStart()
        {
            Interlocked.Exchange(ref _startTime, DateTimeOffset.UtcNow.Ticks);
        }

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
            long date = DateTimeOffset.UtcNow.Ticks;
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
        #endregion

        #region Tracking Properties
        public long CommandsProcessed { get { return _commandsProcessed; } }

        public int EmailsReceived { get { return _emailsReceived; } }

        public int ErrorsGenerated { get { return _errorsGenerated; } }

        public DateTimeOffset LastMailReceivedUTC { get { return new DateTimeOffset(_lastReceivedUtc, TimeSpan.Zero); } }

        public DateTimeOffset LastMailReceivedLocal { get { return LastMailReceivedUTC.ToLocalTime(); } }

        public TimeSpan RunningTime { get { return TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - _startTime); } }

        public int ActiveClients { get { return _activeClients; } }

        public double EmailRate => EmailsReceived / TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - _startTime).TotalMinutes;
        #endregion
    }
}
