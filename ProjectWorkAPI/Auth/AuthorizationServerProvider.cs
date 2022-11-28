using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using ProjectWorkAPI.Models;

namespace ProjectWorkAPI.Auth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            DatabaseEntities DbContext = new DatabaseEntities();

            User u = DbContext.Users.FirstOrDefault(q => q.Username.ToLower() == context.UserName.ToLower());
            if (u == null)
            {
                context.SetError("invalid_grant", "Provided username or password is incorrect");
                return;
            }

            SHA512 sha = SHA512.Create();
            if (u.Password.ToUpper() != BitConverter.ToString(sha.ComputeHash(Encoding.ASCII.GetBytes(context.Password + u.Salt))).Replace("-", "").ToUpper() )
            {
                context.SetError("invalid_grant", "Provided username or password is incorrect");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, u.Role));
            identity.AddClaim(new Claim("username", u.Username));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, u.Id));
            context.Validated(identity);
        }
    }
}
