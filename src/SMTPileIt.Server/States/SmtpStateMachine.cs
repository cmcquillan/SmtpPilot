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
        SmtpCommand _currentCommand = SmtpCommand.NonCommand;
        private readonly EmailStatistics _emailStats;

        public SmtpStateMachine(IMailClient client, SmtpConversation conversation, EmailStatistics statistics)
        {
            _emailStats = statistics;
            _client = client;
            _conversation = conversation;
            _context = new SmtpStateContext(Client, Conversation, _currentCommand, _emailStats);
            CurrentState = new OpenConnectionState();
        }

        public ISmtpStateContext Context { get { return _context; } }

        public IConversationState CurrentState
        {
            get
            { return _currentState; }
            private set
            {
                if (IConversationState.ReferenceEquals(CurrentState, value))
                    return;
                if (_currentState != null)
                    _currentState.LeaveState(_context);
                _currentState = value;
                _currentState.EnterState(_context);
            }
        }

        public SmtpConversation Conversation
        {
            get { return _conversation; }
        }

        public void ProcessLine()
        {
            /* Steps:
             * 1) Check if we have data.  If not, return.
             * 2) Check if there is a new command in the data.
             * 3) If yes:
             *     a) Create a new conversation element.
             *     b) Append to conversation.
             *     c) Check if new command is allowed.
             *     d) If yes, continue to 4.
             * 4) Read a line of conversation element and run ProcessData() on CurrentState.
             * 5) Set new state according to return value of ProcessData().
             */


            if (!Client.HasData)
                return;

            SmtpCommand cmd = Client.PeekCommand();

            if (cmd != SmtpCommand.NonCommand)
            {
                (_context as SmtpStateContext).Command = cmd;
                string line = Client.ReadLine();
                var command = new SmtpCmd(cmd, line);
                Conversation.AddElement(command);

                if(!CurrentState.AllowedCommands.HasFlag(cmd))
                {
                    CurrentState = new ErrorConversationState();
                    return;
                }

                //string line = Client.ReadLine();
                CurrentState = CurrentState.ProcessNewCommand(_context, command, line);
            }
            else
            {
                string line = Client.ReadLine();
                (Conversation.LastElement as IAppendable)?.Append(line);
                CurrentState = CurrentState.ProcessData(_context, line);
            }
        }

        public IMailClient Client { get { return _client; } }

        public bool IsInQuitState { get { return _currentState is QuitConversationState; } }

    }
}
