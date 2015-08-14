using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SMTPileIt.Server.SMTPileIt("127.0.0.1", 25);

            Console.WriteLine("Starting bogus smtp server.");

            server.Start();

            Console.WriteLine("Press 'q' to stop server.");

            while (Console.ReadKey(true).KeyChar != 'q') { }

            Console.WriteLine("Shutting down bogus smtp server.");
            server.Stop();
        }
    }
}
