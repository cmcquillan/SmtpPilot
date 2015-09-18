using NUnit.Framework;
using SMTPileIt.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Tests
{
    [TestFixture]
    public class ClientTests
    {
        SMTPServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new SMTPServer("127.0.0.1", 25027);
            _server.Start();
        }

        [Test]
        public void BasicEmailSendsSuccessfully()
        {
            SmtpClient client = new SmtpClient("127.0.0.1", 25027);

            Assert.DoesNotThrow(() =>
            {
                client.Send("foo@bar.com", "bar@baz.com", "Hello, World", "This is a test.");
            });
            
            client.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
            _server = null;
        }
    }
}
