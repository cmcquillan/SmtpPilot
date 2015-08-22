using SMTPileIt.Server;
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
            var server = new SMTPServer("127.0.0.1", 25);

            Console.WriteLine("Starting bogus smtp server.");

            server.ClientConnected += Server_ClientConnected;
            server.ClientDisconnected += Server_ClientDisconnected;
            server.EmailProcessed += Server_EmailProcessed;
            server.Start();

            Console.WriteLine("Press 'q' to stop server.");

            while (Console.ReadKey(true).KeyChar != 'q') { }

            Console.WriteLine("Shutting down bogus smtp server.");
            server.Stop();
        }

        private static void Server_EmailProcessed(object sender, EmailProcessedEventArgs eventArgs)
        {
            Console.WriteLine("Client processed mail:{0}\tId: {1} {0}\tFrom: {2}", Environment.NewLine, eventArgs.ClientId, eventArgs.Message.FromAddress, Environment.NewLine);
        }

        private static void Server_ClientDisconnected(object sender, MailClientDisconnectedEventArgs eventArgs)
        {
            Console.WriteLine("Client disconnected from server:{0}\tId: {1}{0}\tReason: {2}", Environment.NewLine, eventArgs.ClientId, eventArgs.Reason);
        }

        private static void Server_ClientConnected(object sender, MailClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("Server received new connection:{0}\tId: {1}", Environment.NewLine, eventArgs.ClientId);
        }


    }
}
