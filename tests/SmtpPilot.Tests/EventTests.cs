using NUnit.Framework;
using SmtpPilot.Server;
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
        public void ClientConnectedEventFires()
        {
            bool connected = false;
            Server.ClientConnected += (s, e) =>
            {
                connected = true;
                (s as SMTPServer).Stop();
            };

            Server.Run();
            Assert.True(connected);
        }

        [Test]
        public void ClientDisconnectedEventFires()
        {
            bool disconnected = false;
            Server.ClientDisconnected += (s, e) =>
            {
                disconnected = true;
                (s as SMTPServer).Stop();
            };

            Server.Run();
            Assert.True(disconnected);
        }

        [Test]
        public void MailSentEventFires()
        {
            bool emailSent = false;
            Server.EmailProcessed += (s, e) =>
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
            Server.ClientConnected += (s, e) =>
            {
                (s as SMTPServer).Stop();
            };
            Server.ClientConnected += (s, e) =>
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
            Server.ClientDisconnected += (s, e) =>
            {
                (s as SMTPServer).Stop();
            };
            Server.ClientDisconnected += (s, e) =>
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
            Server.EmailProcessed += (s, e) =>
            {
                Server.Stop();
            };
            Server.EmailProcessed += (s, e) =>
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
