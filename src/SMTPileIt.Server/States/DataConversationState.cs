using System;
using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.Internal;

namespace SMTPileIt.Server.States
{
    public class DataConversationState : IConversationState
    {
        private bool _receivingHeaders;

        public SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.NonCommand;
            }
        }

        public void EnterState(ISmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code354, "Start mail input; end with < CRLF >.< CRLF >"));
            context.Conversation.AddElement(new SmtpData());
            _receivingHeaders = true;
        }

        public void LeaveState(ISmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code250, "Awesome"));
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            if (line.Equals("\r\n"))
            {
                _receivingHeaders = false;
            }
            else if (line.Equals(Constants.EndOfDataElement))
            {
                return new EndDataConversationState();
            }

            if(_receivingHeaders)
            {
                string[] header = line.Split(new char[] { ':' }, 2);
                context.Conversation.AddHeader(new SmtpHeader(header[0], header[1]));
            }


            return this;
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            throw new NotSupportedException();
        }
    }
}