using NUnit.Framework;
using System;
using System.Threading;
using static SmtpPilot.Tests.TestHelper;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class ClientTests
    {
        public ClientTests()
        {
        }

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
