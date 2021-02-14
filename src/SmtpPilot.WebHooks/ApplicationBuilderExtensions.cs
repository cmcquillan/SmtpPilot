using Microsoft.AspNetCore.Http;

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
