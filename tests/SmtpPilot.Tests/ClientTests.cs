using NUnit.Framework;
using SmtpPilot.Server;
using SmtpPilot.Tests.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void BasicEmailSendsSuccessfully()
        {
            Assert.DoesNotThrow(() =>
            {
                var config = TestHelper.GetConfig(TestHelper.BasicMessage);
                var server = new SMTPServer(config);
                server.Events.EmailProcessed += (s, evt) =>
                {
                    server.Stop();  
                };

                server.Run();

                Assert.AreEqual(1, server.Statistics.EmailsReceived);
            });
        }

        [Test]
        public void LargeEmailSendsSuccessfully()
        {
            Assert.DoesNotThrow(() =>
            {
                var config = TestHelper.GetConfig(TestHelper.LongMessage);
                var server = new SMTPServer(config);
                server.Events.EmailProcessed += (s, evt) =>
                {
                    server.Stop();
                };

                server.Run();

                Assert.AreEqual(1, server.Statistics.EmailsReceived);
            });
        }
    }
}
