using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server
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
    /// Delegate that plugs into the <see cref="SMTPServer"/> and is handed to the <see cref="SMTPileIt.Server.States.ISmtpStateContext"/> instance that fires when an email is completed and processed.
    /// </summary>
    /// <param name="sender">The <see cref="SMTPileIt.Server.States.ISmtpStateContext"/> object that fired the event.</param>
    /// <param name="eventArgs">A <see cref="EmailProcessedEventArgs"/> object.</param>
    public delegate void EmailProcessedEventHandler(object sender, EmailProcessedEventArgs eventArgs);

}
