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
        static async Task Main()
        {
            Thread.Sleep(5000);

            var tasks = new List<Task>();

            const int threadCount = 10;

            for (int i = 0; i < threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendMessages));
            }

            while (true) { }
        }

        static async void SendMessages(object none)
        {
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

            await client.ConnectAsync("localhost", 25, MailKit.Security.SecureSocketOptions.None);

            while (true)
            {
                await client.SendAsync(message);
            }

            await client.DisconnectAsync(true);
        }
    }
}
