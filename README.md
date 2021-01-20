# SmtpPilot

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/cmcquillan/SmtpPilot/Run%20.NET%20Tests?label=Build&style=for-the-badge)

SmtpPilot is a mock SMTP server which allows developers to code e-mail functions and applications without the fear of sending to real addresses.  SmtpPilot can run standalone as a server or in-process (if you are developing a .NET application).  The goal of SmtpPilot is to comply with a substantial portion of [RFC 5321](https://tools.ietf.org/html/rfc5321) (as much as makes sense for a mock server) for the purposes of processing e-mail.

## Setting Up a Server

Setting up a basic server is as simple as running a few lines of code in your console application.  

```csharp
var server = new SMTPServer("127.0.0.1", 25);
server.Start();
```

## Configuration of a Server

Accessing into additional features requires initializing a configuration object and passing it to the server's constructor.

```csharp
var config = new SmtpPilotConfiguration("127.0.0.1", 25);
var server = new SMTPServer(config);
server.Start();
```

## Server Events

Hooking into the built-in events requires adding a delegate to the server object. 

```csharp
var server = new SMTPServer("127.0.0.1", 25);
server.EmailProcessed += (s, evt) => Console.WriteLine("New Email from {0}", evt.Message.FromAddress);
server.Start();
```

_Note: Adding new event delegates after the server has started is considered 
thread-safe._

### Available Events
| Event Name | Arguments | Purpose |
| ---------- | --------- | ------- |
| ClientConnected | `object sender`, `MailClientConnectedEventArgs eventArgs` | Called whenever a new client has connected to the server. |
| ClientDisconnected | `object sender`, `MailClientDisconnectedEventArgs eventArgs` | Called whenever a client has disconnected from the server. |
| EmailProcessed | `object sender`, `EmailProcessedEventArgs eventArgs` | Called whenever an email send conversation has been completed. | 

