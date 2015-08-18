using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SMTPileIt.Server;

namespace SMTPileIt.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static SMTPServer _server;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var smtpTester = new SMTPServer("127.0.0.1", SMTPileItConfiguration.DefaultSmtpPort);
            _server = smtpTester;
            _server.Start();
        }
    }
}
