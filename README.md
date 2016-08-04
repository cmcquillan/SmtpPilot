# SmtpPilot

SmtpPilot is a mock SMTP server which allows developers to code e-mail functions and applications without the fear of sending to real addresses.  SmtpPilot can run standalone as a server or in-process (if you are developing a .NET application).  

## Setting Up a Server

Setting up a basic server is as simple as running a few lines of code in your console application.  

    var server = new SMTPServer("127.0.0.1", 25);
    server.Start();

## Configuration of a Server

Hooking into additional features requires initializing a configuration object and passing it to the server's constructor.

    var config = new SmtpPilotConfiguration("127.0.0.1", 25);
    var server = new SMTPServer(config);
    server.Start();

