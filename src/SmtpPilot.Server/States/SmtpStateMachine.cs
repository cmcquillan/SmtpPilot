using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    public class SmtpStateMachine
    {
        private readonly SmtpConversation _conversation;
        private readonly IMailClient _client;
        private IConversationState _currentState;
        private ISmtpStateContext _context;
        SmtpCommand _currentCommand = SmtpCommand.NonCommand;
        private readonly EmailStatistics _emailStats;
        private readonly SmtpPilotConfiguration _configuration;

        public SmtpStateMachine(IMailClient client, SmtpConversation conversation, EmailStatistics statistics, SmtpPilotConfiguration configuration)
        {
            _configuration = configuration;
            _emailStats = statistics;
            _client = client;
            _conversation = conversation;
            _context = new SmtpStateContext(Client, Conversation, _currentCommand, _emailStats, _configuration);
            CurrentState = new OpenConnectionState();
        }

        public ISmtpStateContext Context { get { return _context; } }

        public IConversationState CurrentState
        {
            get
            {
                return _currentState;
            }
            private set
            {
                if (IConversationState.ReferenceEquals(CurrentState, value))
                    return;

                if (_currentState != null)
                {
                    Debug.WriteLine($"Leaving State {_currentState.GetType().Name}", TraceConstants.StateMachine);
                    _currentState.LeaveState(_context);
                    Debug.WriteLine($"Left State {_currentState.GetType().Name}", TraceConstants.StateMachine);
                }
                    
                _currentState = value;

                Debug.WriteLine($"Entering State {_currentState.GetType().Name}", TraceConstants.StateMachine);
                _currentState.EnterState(_context);
                Debug.WriteLine($"Entered State {_currentState.GetType().Name}", TraceConstants.StateMachine);
            }
        }

        public SmtpConversation Conversation
        {
            get { return _conversation; }
        }

        public void ProcessLine()
        {
            /* Steps:
             * 1) Grab a line, exit if null received.
             * 2) If line has a command:
             *     a) Create a new conversation element.
             *     b) Append to conversation.
             *     c) Check if new command is allowed.
             *     d) If yes, continue to 3.
             * 3) Read a line of conversation element and run ProcessData() on CurrentState.
             * 4) Set new state according to return value of ProcessData().
             */

            var line = Client.ReadLine();

            if (line != null)
            {
                SmtpCommand cmd = SmtpCommand.NonCommand;

                string commandString = (line?.Length >= 4) ? line.Substring(0, 4) : String.Empty;
                Enum.TryParse(commandString, out cmd);

                if (!Enum.IsDefined(typeof(SmtpCommand), cmd))
                    cmd = SmtpCommand.NonCommand;

                Debug.WriteLine($"Received command: {cmd}.", TraceConstants.StateMachine);

                if (cmd != SmtpCommand.NonCommand)
                {
                    (_context as SmtpStateContext).Command = cmd;
                    var command = new SmtpCmd(cmd, line);
                    Conversation.AddElement(command);

                    if (!CurrentState.AllowedCommands.HasFlag(cmd))
                    {
                        CurrentState = new ErrorConversationState();
                        return;
                    }

                    CurrentState = CurrentState.ProcessNewCommand(_context, command, line);

                    if (!(CurrentState is ErrorConversationState))
                        _emailStats.AddCommandProcessed();
                }
                else
                {
                    (Conversation.LastElement as IAppendable)?.Append(line);
                    CurrentState = CurrentState.ProcessData(_context, line);
                }
            }
        }

        public IMailClient Client { get { return _client; } }

        public bool IsInQuitState { get { return _currentState is QuitConversationState; } }

    }
}
