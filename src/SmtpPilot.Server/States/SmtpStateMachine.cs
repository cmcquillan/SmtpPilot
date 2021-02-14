using Microsoft.Extensions.Logging;
using SmtpPilot.Server.Communication;
using System;

namespace SmtpPilot.Server.States
{
    internal class SmtpStateMachine
    {
        internal const int MinimumBufferSize = 4096;

        private readonly IServiceProvider _serviceProvider;
        private readonly IMailClient _client;
        private IConversationState _currentState;
        private readonly SmtpStateContext _context;
        private readonly EmailStatistics _emailStats;
        private readonly SmtpPilotConfiguration _configuration;
        private readonly ILogger<SmtpStateMachine> _logger;

        internal SmtpStateMachine(
            IServiceProvider serviceProvider,
            IMailClient client,
            EmailStatistics statistics,
            SmtpPilotConfiguration configuration,
            ILogger<SmtpStateMachine> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _emailStats = statistics;
            _serviceProvider = serviceProvider;
            _client = client;
            _context = new SmtpStateContext(_serviceProvider, _configuration, _client, _emailStats, _configuration.ServerEvents);
            CurrentState = ConversationStates.OpenConnection;
        }

        internal SmtpStateContext Context { get { return _context; } }

        internal IConversationState CurrentState
        {
            get
            {
                return _currentState;
            }
            private set
            {
                if (IConversationState.ReferenceEquals(CurrentState, value))
                    return;

                _currentState = value;

                _logger.LogDebug("Entering State {state}", _currentState);
                _currentState.EnterState(Context);
                _logger.LogDebug("Entered State {state}", _currentState);
            }
        }

        internal void ProcessData()
        {
            try
            {
                var next = CurrentState.Advance(_context);
                TransitionTo(next);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical path exception");
                throw;
            }
        }

        private void TransitionTo(IConversationState newState) => CurrentState = newState;

        internal IMailClient Client { get { return _client; } }

        internal bool IsInQuitState { get { return _currentState is QuitConversationState; } }
    }
}
