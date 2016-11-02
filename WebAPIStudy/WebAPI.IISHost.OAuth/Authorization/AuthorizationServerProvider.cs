using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.IISHost.OAuth.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                var user = context.UserName;
                var password = context.Password;

                if (user == null || password == null)
                {
                    context.SetError(((int)HttpStatusCode.Unauthorized).ToString(), "Application Unauthorized");
                    return Task.FromResult<object>(null);
                }

                // Validate LogOn
                //var _validationResult = _usuarioApp.ValidateLogOn(user, password);

                //if (!_validationResult.IsValid)
                //{
                //    if (CheckResultErrosMessage(_validationResult.Errors, ValidationMessages.UserOrPasswordInvalid))
                //    {
                //        context.SetError(((int)HttpStatusCode.NotFound).ToString(), ValidationMessages.UserOrPasswordInvalid);
                //        return;
                //    }
                //    else if (CheckResultErrosMessage(_validationResult.Errors, ValidationMessages.UserNotActive))
                //    {
                //        context.SetError(((int)HttpStatusCode.Unauthorized).ToString(), ValidationMessages.UserNotActive);
                //        return;
                //    }
                //}

                // Set Token TimeSpan
                context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromHours(8);

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user));
                identity.AddClaim(new Claim(ClaimTypes.PrimarySid, "1"));

                // User Properties
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "user_espec", "123"
                    }
                });

                var roles = new List<string>();
                GenericPrincipal principal = new GenericPrincipal(identity, roles.ToArray());
                var ticket = new AuthenticationTicket(identity, props);
                Thread.CurrentPrincipal = principal;
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                context.SetError(((int)HttpStatusCode.InternalServerError).ToString(), ex.Message);
            }
            
            return Task.FromResult<object>(null);
        }

#pragma warning disable 1998



        //public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        //{
        //    context.Validated();
        //}

        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    try
        //    {
        //        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
        //        var user = context.UserName;
        //        var password = context.Password;

        //        if (user == null || password == null)
        //        {
        //            context.SetError(((int)HttpStatusCode.Unauthorized).ToString(), "Application Unauthorized");
        //            return;
        //        }

        //        // Validate LogOn
        //        //var _validationResult = _usuarioApp.ValidateLogOn(user, password);

        //        //if (!_validationResult.IsValid)
        //        //{
        //        //    if (CheckResultErrosMessage(_validationResult.Errors, ValidationMessages.UserOrPasswordInvalid))
        //        //    {
        //        //        context.SetError(((int)HttpStatusCode.NotFound).ToString(), ValidationMessages.UserOrPasswordInvalid);
        //        //        return;
        //        //    }
        //        //    else if (CheckResultErrosMessage(_validationResult.Errors, ValidationMessages.UserNotActive))
        //        //    {
        //        //        context.SetError(((int)HttpStatusCode.Unauthorized).ToString(), ValidationMessages.UserNotActive);
        //        //        return;
        //        //    }
        //        //}

        //        // Set Token TimeSpan
        //        context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromHours(8);

        //        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
        //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user));
        //        identity.AddClaim(new Claim(ClaimTypes.PrimarySid, "1"));

        //        // User Properties
        //        //StringBuilder _claimsValues = new StringBuilder();
        //        //_claimsValues.Append(usuario.Id.ToString());
        //        //_claimsValues.Append("|");
        //        //_claimsValues.Append(usuario.Login);
        //        //_claimsValues.Append("|");
        //        //_claimsValues.Append(usuario.Email);
        //        //_claimsValues.Append("|");
        //        //var _claimListTemp = identity.Claims.ToList();
        //        //for (int i = 3; i < _claimListTemp.Count; i++)
        //        //{
        //        //    _claimsValues.Append(Convert.ToInt32(Convert.ToBoolean(_claimListTemp[i].Value)).ToString());
        //        //}
        //        var props = new AuthenticationProperties(new Dictionary<string, string>
        //        {
        //            {
        //                "user_espec", "123"
        //            }
        //        });

        //        var roles = new List<string>();
        //        //roles.Add(usuario.Grupo.Nome);
        //        GenericPrincipal principal = new GenericPrincipal(identity, roles.ToArray());
        //        var ticket = new AuthenticationTicket(identity, props);
        //        Thread.CurrentPrincipal = principal;
        //        context.Validated(ticket);
        //    }
        //    catch (Exception ex)
        //    {
        //        context.SetError(((int)HttpStatusCode.InternalServerError).ToString(), ex.Message);
        //    }
        //}

#pragma warning restore 1998

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return base.TokenEndpoint(context);
        }

    }
}