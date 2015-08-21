using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class EmailMessage : IMessage
    {
        private readonly List<SmtpHeader> _headers = new List<SmtpHeader>();
        private IAddress _fromAddress;
        private List<IAddress> _toAddresses = new List<IAddress>();
        private StringBuilder _data = new StringBuilder();

        public IReadOnlyCollection<SmtpHeader> Headers { get { return _headers; } }

        public void AddHeader(SmtpHeader header)
        {
            _headers.Add(header);
        }

        public void AppendLine(string line)
        {
            _data.AppendLine(line);
        }

        public IAddress FromAddress
        {
            get { return _fromAddress; }
            set { _fromAddress = value; }
        }

        public string Data
        {
            get
            {
                return _data.ToString();
            }
        }

        public string DataString
        {
            get
            {
                return Data.ToString();
            }
        }

        public IReadOnlyCollection<IAddress> ToAddresses
        {
            get { return _toAddresses; }
        }

        public void AddAddresses(IAddress[] email)
        {
            _toAddresses.AddRange(email);
        }
    }
}
