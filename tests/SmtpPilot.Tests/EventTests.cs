using Microsoft.AspNetCore.Connections;
using NUnit.Framework;
using SmtpPilot.Server;
using SmtpPilot.Server.Communication;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SmtpPilot.Tests.TestHelper;

namespace SmtpPilot.Tests
{
    [TestFixture]
    public class EventTests
    {
        private CancellationTokenSource _cts;

        protected SMTPServer Server { get; set; }

        [SetUp]
        public void SetupServer()
        {
            Server = GetServer();
            _cts = new CancellationTokenSource();
            _cts.CancelAfter(TimeSpan.FromSeconds(30));
        }

        [Test]
        public async Task ClientConnectedEventPassesServerAsSender()
        {
            bool connected = false;
            Server.Events.ClientConnected += (s, e) =>
            {
                connected = true;
                _cts.Cancel();
            };

            await SendAndRun(BasicMessage, Server, _cts.Token);
            Assert.True(connected);
        }

        [Test]
        public async Task ClientConnectedEventFires()
        {
            bool connected = false;
            Server.Events.ClientConnected += (s, e) =>
            {
                connected = true;
                _cts.Cancel();
            };
            
            await SendAndRun(BasicMessage, Server, _cts.Token);

            Assert.True(connected);
        }

        [Test]
        public async Task ClientDisconnectedEventFires()
        {
            bool disconnected = false;
            Server.Events.ClientDisconnected += (s, e) =>
            {
                disconnected = true;
                _cts.Cancel();
            };

            await SendAndRunThenDisconnect(BasicMessage, Server, _cts.Token);

            Assert.True(disconnected);
        }

        [Test]
        public async Task MailSentEventFires()
        {
            bool emailSent = false;
            Server.Events.EmailProcessed += (s, e) =>
            {
                emailSent = true;
                _cts.Cancel();
            };

            await SendAndRun(BasicMessage, Server, _cts.Token);

            Assert.True(emailSent);
        }

        [Test]
        public void ExceptionInConnectedEventDoesNotInterruptServer()
        {
            bool eventFired = false;
            Server.Events.ClientConnected += (s, e) =>
            {
                _cts.Cancel();
            };
            Server.Events.ClientConnected += (s, e) =>
            {
                eventFired = true;
                throw new Exception("I'm a bad monkey.");
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                await SendAndRun(BasicMessage, Server, _cts.Token);
            });

            Assert.True(eventFired);
        }

        [Test]
        public void ExceptionInDisconnectedEventDoesNotInterruptServer()
        {
            bool eventFired = false;
            Server.Events.ClientDisconnected += (s, e) =>
            {
                _cts.Cancel();
            };
            Server.Events.ClientDisconnected += (s, e) =>
            {
                eventFired = true;
                throw new Exception("I'm a bad monkey.");
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                await SendAndRunThenDisconnect(BasicMessage, Server, _cts.Token);
            });

            Assert.True(eventFired);
        }

        [Test]
        public void ExceptionInMailEventDoesNotInterruptServer()
        {
            bool eventFired = false;
            Server.Events.EmailProcessed += (s, e) =>
            {
                _cts.Cancel();
            };
            Server.Events.EmailProcessed += (s, e) =>
            {
                eventFired = true;
                throw new Exception("I'm a bad monkey.");
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                await SendAndRun(BasicMessage, Server, _cts.Token);
            });

            Assert.True(eventFired);
        }

        private static SMTPServer GetServer()
        {
            var config = GetConfig();
            var server = SMTPServer.CreateSmtpHost(Array.Empty<string>(), config);
            return server;
        }


    }
}
