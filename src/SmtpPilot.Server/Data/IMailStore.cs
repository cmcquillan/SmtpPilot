using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Data
{
    public interface IMailStore 
    {
        void SaveMessage(IMessage message);
    }
}
