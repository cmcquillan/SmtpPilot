using NUnit.Framework;
using SmtpPilot.Server;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Tests
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
        public void ClientConnectedEventPassesServerAsSender()
        {
            SMTPServer theServer = null;
            Server.Events.ClientConnected += (s, e) =>
            {
                theServer = s as SMTPServer;
                Server.Stop();
            };

            Server.Run();
            Assert.IsNotNull(theServer);
            Assert.IsInstanceOf<SMTPServer>(theServer);
        }

        [Test]
        public void ClientConnectedEventFires()
        {
            bool connected = false;
            Server.Events.ClientConnected += (s, e) =>
            {
                connected = true;
                Server.Stop();
            };

            Server.Run();
            Assert.True(connected);
        }

        [Test]
        public void ClientDisconnectedEventPassesServerAsSender()
        {
            SMTPServer theServer = null;
            Server.Events.ClientDisconnected += (s, e) =>
            {
                theServer = s as SMTPServer;
                Server.Stop();
            };

            Server.Run();
            Assert.IsNotNull(theServer);
            Assert.IsInstanceOf<SMTPServer>(theServer);
        }

        [Test]
        public void ClientDisconnectedEventFires()
        {
            bool disconnected = false;
            Server.Events.ClientDisconnected += (s, e) =>
            {
                disconnected = true;
                Server.Stop();
            };

            Server.Run();
            Assert.True(disconnected);
        }

        [Test]
        public void MailSentEventPassesClientAsSender()
        {
            IMailClient theClient = null;
            Server.Events.EmailProcessed += (s, e) =>
            {
                theClient = s as IMailClient;
                Server.Stop();
            };

            Server.Run();
            Assert.IsNotNull(theClient);
            Assert.IsInstanceOf<IMailClient>(theClient);
        }

        [Test]
        public void MailSentEventFires()
        {
            bool emailSent = false;
            Server.Events.EmailProcessed += (s, e) =>
            {
                emailSent = true;
                Server.Stop();
            };

            Server.Run();
            Assert.True(emailSent);
        }

        [Test]
        public void ExceptionInConnectedEventDoesNotInterruptServer()
        {
            bool eventFired = false;
            Server.Events.ClientConnected += (s, e) =>
            {
                Server.Stop();
            };
            Server.Events.ClientConnected += (s, e) =>
            {
                eventFired = true;
                throw new Exception("I'm a bad monkey.");
            };

            Assert.DoesNotThrow(() =>
            {
                Server.Run();
            });

            Assert.True(eventFired);
        }

        [Test]
        public void ExceptionInDisconnectedEventDoesNotInterruptServer()
        {
            bool eventFired = false;
            Server.Events.ClientDisconnected += (s, e) =>
            {
                Server.Stop();
            };
            Server.Events.ClientDisconnected += (s, e) =>
            {
                eventFired = true;
                throw new Exception("I'm a bad monkey.");
            };

            Assert.DoesNotThrow(() =>
            {
                Server.Run();
            });

            Assert.True(eventFired);
        }

        [Test]
        public void ExceptionInMailEventDoesNotInterruptServer()
        {
            bool eventFired = false;
            Server.Events.EmailProcessed += (s, e) =>
            {
                Server.Stop();
            };
            Server.Events.EmailProcessed += (s, e) =>
            {
                eventFired = true;
                throw new Exception("I'm a bad monkey.");
            };

            Assert.DoesNotThrow(() =>
            {
                Server.Run();
            });

            Assert.True(eventFired);
        }

        private SMTPServer GetServer()
        {
            var config = TestHelper.GetConfig(TestHelper.BasicMessage);
            var server = new SMTPServer(config);
            return server;
        }
    }
}
