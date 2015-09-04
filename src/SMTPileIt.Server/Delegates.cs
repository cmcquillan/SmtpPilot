﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    public delegate void MailClientConnectedEventHandler(object sender, MailClientConnectedEventArgs eventArgs);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    public delegate void MailClientDisconnectedEventHandler(object sender, MailClientDisconnectedEventArgs eventArgs);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    public delegate void EmailProcessedEventHandler(object sender, EmailProcessedEventArgs eventArgs);

}