using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class SmtpHeader
    {
        public const string DATE_HEADER = @"Date";
        public const string FROM_HEADER = @"From";
        public const string TO_HEADER = @"To";
        public const string SUBJECT_HEADER = @"Subject";

        public SmtpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Name, Value);
        }
    }
}
