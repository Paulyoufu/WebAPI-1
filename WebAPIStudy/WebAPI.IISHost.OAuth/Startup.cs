using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using WebAPI.Core.Config;
using Microsoft.Owin.Security.OAuth;
using WebAPI.IISHost.OAuth.Authorization;

[assembly: OwinStartup(typeof(WebAPI.IISHost.OAuth.Startup))]

namespace WebAPI.IISHost.OAuth
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration();

            WebApiConfig.Register(HttpConfiguration);
            ConfigureOAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(HttpConfiguration);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // token generation
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/security/token"),
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            
            // token consume
            var beareropt = new OAuthBearerAuthenticationOptions();
            beareropt.Provider = new CustomBearerAuthenticationProvider();
            app.UseOAuthBearerAuthentication(beareropt);


        }
    }
}
