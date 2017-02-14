using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server
{
    public class SmtpServerEvents
    {
        public event MailClientConnectedEventHandler ClientConnected;

        public event MailClientDisconnectedEventHandler ClientDisconnected;

        public event ServerStartedEventHandler ServerStarted;

        public event ServerStoppedEventHandler ServerStopped;

        public event EmailProcessedEventHandler EmailProcessed;

        internal void OnEmailProcessed(object sender, EmailProcessedEventArgs eventArgs)
        {
            EmailProcessedEventHandler handler = EmailProcessed;

            if (handler != null)
            {
                foreach (EmailProcessedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(sender, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        internal void OnClientConnected(object sender, MailClientConnectedEventArgs eventArgs)
        {
            MailClientConnectedEventHandler handler = ClientConnected;

            if (handler != null)
            {
                foreach (MailClientConnectedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(sender, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        internal void OnClientDisconnected(object sender, MailClientDisconnectedEventArgs eventArgs)
        {
            MailClientDisconnectedEventHandler handler = ClientDisconnected;

            if (handler != null)
            {
                foreach (MailClientDisconnectedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(sender, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        internal void OnServerStart(object sender, ServerEventArgs eventArgs)
        {
            ServerStartedEventHandler handler = ServerStarted;

            if (handler != null)
            {
                foreach (ServerStartedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(sender, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        internal void OnServerStop(object sender, ServerEventArgs eventArgs)
        {
            ServerStoppedEventHandler handler = ServerStopped;

            if (handler != null)
            {
                foreach (ServerStoppedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(sender, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }
    }
}
