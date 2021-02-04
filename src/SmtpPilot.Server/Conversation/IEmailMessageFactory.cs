using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server.Conversation
{
    public interface IEmailMessageFactory
    {
        IMessage CreateNewMessage();
    }
}
