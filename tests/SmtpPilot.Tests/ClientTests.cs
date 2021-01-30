using NUnit.Framework;
using SmtpPilot.Server;
using SmtpPilot.Tests.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SmtpPilot.Tests.TestHelper;

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
                cts.CancelAfter(30000);
                var config = GetConfig();
                var server = SMTPServer.CreateSmtpHost(Array.Empty<string>(), config);

                server.Events.EmailProcessed += (s, evt) =>
                {
                    cts.Cancel();
                };

                await SendAndRun(BasicMessage, server, cts.Token);

                Assert.AreEqual(1, server.Statistics.EmailsReceived);
            });
        }

        [Test]
        public void LargeEmailSendsSuccessfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(30000);
                var config = GetConfig();
                var server = SMTPServer.CreateSmtpHost(Array.Empty<string>(), config);
                server.Events.EmailProcessed += (s, evt) =>
                {
                    cts.Cancel();
                };

                await SendAndRun(LongMessage, server, cts.Token);

                Assert.AreEqual(1, server.Statistics.EmailsReceived);
            });
        }
    }
}
