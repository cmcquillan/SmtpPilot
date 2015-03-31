using SMTPileIt.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.IO
{
    public static class IOHelper
    {
        private const char ASCIISpace = (char)0x20;
        private const char ASCIICarriageReturn = (char)0x0D;
        private const char ASCIILineFeed = (char)0x0A;

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

        public static string GetLineFromBuffer(char[] buffer, int offset, int limit)
        {
            bool carriageReturnFound = false;
            int i, n;

            for(i = offset, n = 0; i < offset + limit; i++, n++)
            {
                if (buffer[i] == ASCIICarriageReturn)
                {
                    carriageReturnFound = true;
                    continue;
                }

                if (buffer[i] == ASCIILineFeed && carriageReturnFound)
                    break;

                carriageReturnFound = false;
            }

            n++;
            char[] strBuf = new char[n];

            Array.Copy(buffer, offset, strBuf, 0, n);
            return new string(strBuf);
        }
    }
}
