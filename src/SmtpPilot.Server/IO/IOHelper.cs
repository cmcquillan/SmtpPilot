using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmtpPilot.Server.IO
{
    public static class IOHelper
    {
        static IOHelper()
        {
            _mailFromRegex = new Regex(Constants.EmailAddressRegex, RegexOptions.Singleline|RegexOptions.Compiled);
        }

        private const char ASCIISpace = (char)0x20;
        private const char ASCIICarriageReturn = (char)0x0D;
        private const char ASCIILineFeed = (char)0x0A;
        private static readonly Regex _mailFromRegex;

        public static SmtpCommand GetCommand(IEnumerable<char> input)
        {
            char[] cmd = new char[4];

            for (int i = 0; i < input.Count(); i++)
            {
                if (input.ElementAt(i) == ASCIISpace)
                    break;

                cmd[i] = input.ElementAt(i);
            }

            string commandText = new string(cmd);

            SmtpCommand command = SmtpCommand.NonCommand;
            Enum.TryParse<SmtpCommand>(commandText, out command);

            return command;
        }

        public static bool LooksLikeHeader(string line)
        {
            /*
             * Looking for a space or a colon.  If we find
             * a space before finding the colon, then assume
             * we do not have a header.
             * If we never find a colon, then it was definitely not
             * a header.
             */
            for(int i = 0; i < line.Length; i++)
            {
                if (Char.Equals(line[i], ASCIISpace))
                    return false;

                if (Char.Equals(line[i], ':'))
                    return true;
            }

            return false;
        }

        public static string[] ParseEmails(string s)
        {
            var matches = _mailFromRegex.Matches(s);
            string[] emails = new string[matches.Count];

            for(int i = 0; i < matches.Count; i++)
            {
                emails[i] = matches[i].Value;
            }

            return emails;
        }


    }
}
