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
    class EventTests
    {
        SMTPServer Server { get; set; }

        [SetUp]
        public void SetupServer()
        {
            Server = GetServer();
        }

        [TearDown]
        public void TearDownServer()
        {
            Server.Stop();
        }

        [Test]
        public void ClientConnectedEventFires()
        {
            Server.ClientConnected += TestConnectMethod;
            Server.Start();
            try
            {
                SendTestEmail();
            }
            catch (Exception) /* This is going to throw since the server will abort */
            { }
        }

        [Test]
        public void ClientDisconnectedEventFires()
        {
            Server.ClientDisconnected += TestDisconnectMethod;
            Server.Start();
            SendTestEmail();
        }

        [Test]
        public void MailSentEventFires()
        {
            Server.EmailProcessed += TestMailSentMethod;
            Server.Start();
            SendTestEmail();
        }

        [Test]
        public void ExceptionInConnectedEventDoesNotInterruptServer()
        {
            Server.ClientConnected += ExceptionEventMethod;
            Assert.DoesNotThrow(() =>
            {
                Server.Start();
                SendTestEmail();
            });
        }

        [Test]
        public void ExceptionInDisconnectedEventDoesNotInterruptServer()
        {
            Server.ClientDisconnected += ExceptionEventMethod;
            Assert.DoesNotThrow(() =>
            {
                Server.Start();
                SendTestEmail();
            });
        }

        [Test]
        public void ExceptionInMailEventDoesNotInterruptServer()
        {
            Server.EmailProcessed += ExceptionEventMethod;
            Assert.DoesNotThrow(() =>
            {
                Server.Start();
                SendTestEmail();
            });
        }

        private void ExceptionEventMethod(object sender, MailClientEventArgs eventArgs)
        {
            throw new InvalidOperationException("We done bad.");
        }

        private void TestMailSentMethod(object sender, EmailProcessedEventArgs eventArgs)
        {
            Assert.Pass("Successfully fired the mail sent method");
        }

        private void TestDisconnectMethod(object sender, MailClientDisconnectedEventArgs eventArgs)
        {
            Assert.Pass("Successfully fired the client disconnected method");
        }

        private void TestConnectMethod(object sender, MailClientConnectedEventArgs eventArgs)
        {
            Assert.Pass("Successfully fired the client connected method");
        }

        private SMTPServer GetServer()
        {
            var server = new SMTPServer("127.0.0.1", 25026);
            return server;
        }

        private void SendTestEmail()
        {
            using (var client = new SmtpClient("127.0.0.1", 25026))
            {
                var message = new MailMessage("foo@bar.com", "bar@baz.com");
                message.Subject = "Hello, World";
                message.Body = "This is my message";
                client.Send(message);
            }
        }
    }
}
