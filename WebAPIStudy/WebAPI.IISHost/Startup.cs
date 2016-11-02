using Microsoft.Owin;
using Owin;
using System.Web.Http;
using WebAPI.Core.Config;

[assembly: OwinStartup(typeof(WebAPI.IISHost.Startup))]

namespace WebAPI.IISHost
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration();

            WebApiConfig.Register(HttpConfiguration);

            app.UseWebApi(HttpConfiguration);
        }
    }
}
