using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace SmtpPilot.Server.IO
{
    internal static class IOHelper
    {
        static IOHelper()
        {
            _mailFromRegex = new Regex(Constants.EmailAddressRegex, RegexOptions.Singleline | RegexOptions.Compiled);
        }

        private const char _space = (char)0x20;
        private static readonly Regex _mailFromRegex;

        internal static SmtpCommand GetCommand(ReadOnlySpan<char> input)
        {
            char[] cmd = new char[4];

            for (int i = 0; i < Math.Min(4, input.Length); i++)
            {
                if (input[i] == _space)
                    break;

                cmd[i] = input[i];
            }

            string commandText = new string(cmd);

            Enum.TryParse(commandText, out SmtpCommand command);

            return command;
        }

        internal static bool LooksLikeHeader(ReadOnlySpan<char> line)
        {
            /*
             * Looking for a space or a colon.  If we find
             * a space before finding the colon, then assume
             * we do not have a header.
             * If we never find a colon, then it was definitely not
             * a header.
             */
            for (int i = 0; i < line.Length; i++)
            {
                if (Char.Equals(line[i], _space))
                    return false;

                if (Char.Equals(line[i], ':'))
                    return true;
            }

            return false;
        }

        internal static string[] ParseEmails(string s)
        {
            var matches = _mailFromRegex.Matches(s);
            string[] emails = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                emails[i] = matches[i].Value;
            }

            return emails;
        }

        internal static bool IsEmptyOrWhitespace(this ReadOnlySpan<char> span)
        {
            if (span.IsEmpty)
                return true;

            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == '\0')
                    return true;

                if (!Char.IsWhiteSpace(span[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
