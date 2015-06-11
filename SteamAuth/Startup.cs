using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.OpenID;
using Owin.Security.Providers.Steam;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(SteamAuth.Startup))]

namespace SteamAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ExternalCookie",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = ".AspNet.ExternalCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(5)
            });

            var webconfig = WebConfigurationManager.OpenWebConfiguration("~/");
            var key = webconfig.AppSettings.Settings["apiKey"].Value;

            var options = new SteamAuthenticationOptions
            {
                ApplicationKey = key
            };

            app.UseSteamAuthentication(options);
        }
    }
}
