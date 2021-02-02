using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Conversation
{
    public readonly struct SmtpHeader
    {
        public const string DATE_HEADER = @"Date";
        public const string FROM_HEADER = @"From";
        public const string TO_HEADER = @"To";
        public const string SUBJECT_HEADER = @"Subject";

        public static SmtpHeader Parse(ReadOnlySpan<char> choppedLine)
        {
            var splitIndex = choppedLine.IndexOf(':');

            return new SmtpHeader(
                choppedLine[0..splitIndex].ToString(),
                choppedLine[(splitIndex + 2)..].ToString());
        }

        public SmtpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public readonly string Name { get; }

        public readonly string Value { get; }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Name, Value);
        }
    }
}
