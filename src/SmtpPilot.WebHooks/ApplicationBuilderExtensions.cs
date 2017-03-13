using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSmtpPilotWebHooks(this IApplicationBuilder app, string path = "/smtppilot")
        {
            app.Map(new PathString(path), builder =>
            {
                builder.UseMvc();
            });

            return app;
        }
    }
}
