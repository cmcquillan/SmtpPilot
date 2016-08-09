using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Conversation
{
    public class EmailMessage : IMessage
    {
        private readonly List<SmtpHeader> _headers = new List<SmtpHeader>();
        private IAddress _fromAddress;
        private List<IAddress> _toAddresses = new List<IAddress>();
        private StringBuilder _data = new StringBuilder();
        private bool _complete = false;

        public ReadOnlyCollection<SmtpHeader> Headers { get { return _headers.AsReadOnly(); } }

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

        public ReadOnlyCollection<IAddress> ToAddresses
        {
            get { return _toAddresses.AsReadOnly(); }
        }

        public bool IsComplete
        {
            get
            {
                return _complete;
            }
        }

        public void AddAddresses(IAddress[] email)
        {
            _toAddresses.AddRange(email);
        }

        public void Complete()
        {
            _complete = true;
        }
    }
}
