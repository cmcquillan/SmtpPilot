using NUnit.Framework;
using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Tests
{
    [TestFixture]
    public class IOTests
    {
        [Test]
        [TestCase("<cmcquillan@gmail.com>", 1)]
        [TestCase("Casey McQuillan <cmcquillan@gmail.com>", 1)]
        [TestCase("Casey McQuillan <cmcquillan@gmail.com>; John Smith <jsmith@gmail.com>", 2)]
        public void ParseEmailCapturesValidAmountOfEmails(string s, int numEmails)
        {
            string[] emails = IOHelper.ParseEmails(s);

            Assert.AreEqual(numEmails, emails.Length);
        }


        [Test]
        public void GetLineFromBufferRetrievesLineData()
        {
            string testString = "This is the first line\r\nThis is the second line.\r\n";

            string line = IOHelper.GetLineFromBuffer(testString.ToCharArray(), 0, testString.Length);

            Assert.AreEqual("This is the first line\r\n", line);
        }

        [Test]
        public void GetLineFromBufferSkipsOrphanedCarriageReturns()
        {
            string testString = "This is\r an \r errant line\r\nThis is the second line.\r\n";

            string line = IOHelper.GetLineFromBuffer(testString.ToCharArray(), 0, testString.Length);

            Assert.AreEqual("This is\r an \r errant line\r\n", line);
        }

        [Test]
        public void GetLineFromBufferRetrievesLineDataWithOffset()
        {
            string testString = "This is the first line\r\nThis is the second line.\r\n";

            string line = IOHelper.GetLineFromBuffer(testString.ToCharArray(), 24, testString.Length - 24);

            Assert.AreEqual("This is the second line.\r\n", line);
        }

        [Test]
        public void GetCommandParsesSmtpCommands()
        {
            string testString = "HELO Ord-Mantell";
            SmtpCommand cmd = IOHelper.GetCommand(testString);
            Assert.AreEqual(SmtpCommand.HELO, cmd);
        }
    }
} 
