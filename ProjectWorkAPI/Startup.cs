using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ProjectWorkAPI.Auth;
using System;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(ProjectWorkAPI.Startup))]

namespace ProjectWorkAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //enable cors origin requests
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var AuthProvider = new AuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/v1/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                Provider = AuthProvider
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
