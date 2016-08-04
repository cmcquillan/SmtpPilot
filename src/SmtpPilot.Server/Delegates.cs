using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server
{
    /// <summary>
    /// Delegate that plugs into <see cref="SMTPServer"/> that fires when a new client connects to the server.
    /// </summary>
    /// <param name="sender">The <see cref="SMTPServer"/> instance that receives the client connection.</param>
    /// <param name="eventArgs">A <see cref="MailClientConnectedEventArgs"/> object.</param>
    public delegate void MailClientConnectedEventHandler(object sender, MailClientConnectedEventArgs eventArgs);

    /// <summary>
    /// Delegate that plugs into <see cref="SMTPServer"/> that fires when a client disconnects from the server.
    /// </summary>
    /// <param name="sender">The <see cref="SMTPServer"/> instance from which the client disconnected.</param>
    /// <param name="eventArgs">A <see cref="MailClientDisconnectedEventArgs"/> object.</param>
    public delegate void MailClientDisconnectedEventHandler(object sender, MailClientDisconnectedEventArgs eventArgs);

    /// <summary>
    /// Delegate that plugs into the <see cref="SMTPServer"/> and is handed to the <see cref="SmtpPilot.Server.States.ISmtpStateContext"/> instance that fires when an email is completed and processed.
    /// </summary>
    /// <param name="sender">The <see cref="SmtpPilot.Server.States.ISmtpStateContext"/> object that fired the event.</param>
    /// <param name="eventArgs">A <see cref="EmailProcessedEventArgs"/> object.</param>
    public delegate void EmailProcessedEventHandler(object sender, EmailProcessedEventArgs eventArgs);

    /// <summary>
    /// Delegate that plugs into the <see cref="SMTPServer"/> and is fired when the server is started.  This is fired as soon as the worker thread is initialized and running.
    /// </summary>
    /// <remarks>Server start time is rather minimal, so most applications will not need explicit access to this information.  
    /// Use this if you application needs precise timing knowledge of when the server is ready and listening.</remarks>
    /// <param name="sender">A reference to <see cref="SMTPServer"/> firing the event.</param>
    /// <param name="eventArgs">A <see cref="ServerEventArgs"/> object.</param>
    public delegate void ServerStartedEventHandler(object sender, ServerEventArgs eventArgs);

    /// <summary>
    /// Delegate that plugs into the <see cref="SMTPServer"/> and is fired when the server is stopped.  
    /// This is fired as soon as the worker thread has completed its final loop, right before releasing it back to the thread pool.
    /// </summary>
    /// <param name="sender">A reference to <see cref="SMTPServer"/> firing the event.</param>
    /// <param name="eventArgs">A <see cref="ServerEventArgs"/> object.</param>
    public delegate void ServerStoppedEventHandler(object sender, ServerEventArgs eventArgs);

}
