using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot.Client
{
    class Program
    {
        static void Main()
        {
            Thread.Sleep(5000);

            using var client = new SmtpClient
            {
                LocalDomain = "localhost",
                Timeout = Int32.MaxValue
            };

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("testfrom@test.com"));
            message.To.Add(new MailboxAddress("testto@test.com"));
            message.Subject = "Hello, World";
            message.Body = new TextPart("plain") { Text = "This is my message." };

            client.Connect("localhost", 25, MailKit.Security.SecureSocketOptions.None);

            for (int i = 0; i < 200; i++)
            {
                client.Send(message);
            }

            client.Disconnect(true);
        }
    }
}
