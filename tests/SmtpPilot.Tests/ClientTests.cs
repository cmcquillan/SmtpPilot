using NUnit.Framework;
using SmtpPilot.Server;
using SmtpPilot.Tests.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void BasicEmailSendsSuccessfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var cts = new CancellationTokenSource();
                var config = TestHelper.GetConfig(TestHelper.BasicMessage);
                var server = new SMTPServer(config);
                server.Events.EmailProcessed += (s, evt) =>
                {
                    cts.Cancel();
                };

                await server.Run(cts.Token);

                Assert.AreEqual(1, server.Statistics.EmailsReceived);
            });
        }

        [Test]
        public void LargeEmailSendsSuccessfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var cts = new CancellationTokenSource();
                var config = TestHelper.GetConfig(TestHelper.LongMessage);
                var server = new SMTPServer(config);
                server.Events.EmailProcessed += (s, evt) =>
                {
                    cts.Cancel();
                };

                await server.Run(cts.Token);

                Assert.AreEqual(1, server.Statistics.EmailsReceived);
            });
        }
    }
}
