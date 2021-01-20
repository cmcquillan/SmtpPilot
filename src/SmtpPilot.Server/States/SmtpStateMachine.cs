using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    internal class SmtpStateMachine
    {
        internal const int MinimumBufferSize = 4096;

        private readonly ArrayPool<char> _arrayPool = ArrayPool<char>.Shared;
        private readonly SmtpConversation _conversation;
        private readonly IMailClient _client;
        private IConversationState _currentState;
        private readonly ISmtpStateContext _context;
        private readonly SmtpCommand _currentCommand = SmtpCommand.NonCommand;
        private readonly EmailStatistics _emailStats;
        private readonly SmtpPilotConfiguration _configuration;

        internal SmtpStateMachine(IMailClient client, SmtpConversation conversation, EmailStatistics statistics, SmtpPilotConfiguration configuration)
        {
            _configuration = configuration;
            _emailStats = statistics;
            _client = client;
            _conversation = conversation;
            _context = new SmtpStateContext(Client, Conversation, _currentCommand, _emailStats, _configuration);
            CurrentState = ConversationStates.OpenConnection;
        }

        internal ISmtpStateContext Context { get { return _context; } }

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

        internal SmtpConversation Conversation
        {
            get { return _conversation; }
        }

        internal async Task ProcessData()
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
            char[] buffer = null;

            try
            {
                buffer = _arrayPool.Rent(MinimumBufferSize);
                Memory<char> memory = buffer.AsMemory();
                var read = Client.ReadLine(memory.Span);

                //var line = await Client.ReadLine();

                if (read > 0)
                {
                    SmtpCmd command = null;

                    if (CurrentState.AcceptingCommands)
                    {
                        command = GetCommandFromLine(memory.Span.Slice(0, read));
                        (_context as SmtpStateContext).Command = command.Command;
                        Conversation.AddElement(command);

                        if (!CurrentState.AllowedCommands.HasFlag(command.Command))
                        {
                            CurrentState = new ErrorConversationState();
                            return;
                        }

                        if (!(CurrentState is ErrorConversationState))
                            _emailStats.AddCommandProcessed();
                    }
                    else
                    {
                        (Conversation.LastElement as IAppendable)?.Append(memory.Span.Slice(0, read).ToString());
                    }

                    CurrentState = CurrentState.ProcessData(_context, command, memory.Span.Slice(0, read));
                }
            } 
            finally
            {
                _arrayPool.Return(buffer);
            }
        }

        private static SmtpCmd GetCommandFromLine(Span<char> line)
        {
            SmtpCmd command;
            string commandString = (line.Length >= 4) ? line.Slice(0, 4).ToString() : String.Empty;
            Enum.TryParse(commandString, out SmtpCommand cmd);

            if (!Enum.IsDefined(typeof(SmtpCommand), cmd))
                cmd = SmtpCommand.NonCommand;

            Debug.WriteLine($"Received command: {cmd}.", TraceConstants.StateMachine);


            command = new SmtpCmd(cmd, line.ToString());
            return command;
        }

        internal IMailClient Client { get { return _client; } }

        internal bool IsInQuitState { get { return _currentState is QuitConversationState; } }

    }
}
