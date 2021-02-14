using NUnit.Framework;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;

namespace SmtpPilot.Tests
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
        public void GetCommandParsesSmtpCommands()
        {
            string testString = "HELO Ord-Mantell";
            SmtpCommand cmd = IOHelper.GetCommand(testString);
            Assert.AreEqual(SmtpCommand.HELO, cmd);
        }
    }
}
