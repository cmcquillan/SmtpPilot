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

            //using (var client = new SmtpClient("localhost", 25))
            using (var client = new SmtpClient())
            {
                //client.Credentials = new NetworkCredential("cmcquillan", "password");
                //client.UseDefaultCredentials = false;
                var message = new MailMessage("casey.mcquillan@gmail.com", "cmcquillan@restaurant.org");
                message.To.Add(new MailAddress("casey.r.mcquillan@gmail.com"));
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
