using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace WebAPI.IISHost.OAuth.Authorization
{
    public class CustomBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            return base.ValidateIdentity(context);
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get("Authorization");
            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }
            return base.RequestToken(context);
        }
    }
}
