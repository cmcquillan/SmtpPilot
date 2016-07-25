using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);

            using (var client = new SmtpClient())
            {
                var message = new MailMessage("testfrom@test.com", "testto@test.com");
                message.Subject = "Hello, World";
                message.Body = "This is my message";

                client.Send(message);
                client.Send(message);
                client.Send(message);
                client.Send(message);
            }
        }
    }
}
