using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.States
{
    public class SmtpStateMachine
    {
        private readonly SmtpConversation _conversation;
        private readonly IMailClient _client;
        private IConversationState _currentState;
        private ISmtpStateContext _context;

        public SmtpStateMachine(IMailClient client, SmtpConversation conversation)
        {
            _client = client;
            _conversation = conversation;
            CurrentState = new OpenConnectionState();
        }

        public IConversationState CurrentState
        {
            get
            { return _currentState; }
            private set
            {
                if (_currentState != null)
                    _currentState.LeaveState(_context);
                _currentState = value;
                _currentState.EnterState(Client);
            }
        }

        public SmtpConversation Conversation
        {
            get { return _conversation; }
        }

        public void ProcessLine()
        {
            if (!Client.HasData)
                return;

            SmtpCommand cmd = Client.PeekCommand();

            _context = new SmtpStateContext(Client, Conversation, cmd);

            if (CurrentState.AllowedCommands.HasFlag(cmd))
            {
                var newState = CurrentState.Process(_context);
                if (!IConversationState.ReferenceEquals(newState, CurrentState))
                    CurrentState = newState;
            }
                
            else
                Client.Write("Error!");
        }

        public IMailClient Client { get { return _client; } }

        public bool IsInQuitState { get { return _currentState is QuitState; } }


    }
}
