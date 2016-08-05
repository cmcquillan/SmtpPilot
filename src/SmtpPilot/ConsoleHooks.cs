using SmtpPilot.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal static class ConsoleHooks
    {
        internal static void Server_EmailProcessed(object sender, EmailProcessedEventArgs eventArgs)
        {
            LogInfo($"Client processed mail.{Environment.NewLine}\tId: {eventArgs.ClientId} {Environment.NewLine}\tFrom: {eventArgs.Message.FromAddress}");
        }

        internal static void Server_ClientDisconnected(object sender, MailClientDisconnectedEventArgs eventArgs)
        {
            LogInfo($"Client disconnected from server.{Environment.NewLine}\tId: {eventArgs.ClientId}{Environment.NewLine}\tReason: {eventArgs.Reason}");
        }

        internal static void Server_ClientConnected(object sender, MailClientConnectedEventArgs eventArgs)
        {
            LogInfo($"Server received new connection.{Environment.NewLine}\tId: {eventArgs.ClientId}");
        }

        internal static void Server_Stopped(object sender, ServerEventArgs eventArgs)
        {
            LogInfo($"Server stopped listening at {eventArgs.EventTime}.");
        }

        internal static void Server_Started(object sender, ServerEventArgs eventArgs)
        {
            LogInfo($"Server began listening at {eventArgs.EventTime}.");
        }

        internal static void LogInfo(string text)
        {
            Console.WriteLine($"({DateTime.Now:O}): {text}");
        }
    }
}
