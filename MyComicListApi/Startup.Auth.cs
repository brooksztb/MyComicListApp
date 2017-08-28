using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using System.Text;
using MyComicListApi.CustomTokenAuthProvider;

namespace MyComicListApi
{
    public partial class Startup
    {
        //private void ConfigureAuth(IApplicationBuilder app)
        //{
        //    //var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));

        //    //var tokenValidationParameters = new TokenValidationParameters
        //    //{
        //    //    //Signing Key Must Match
        //    //    ValidateIssuerSigningKey = true,
        //    //    IssuerSigningKey = signingKey,
        //    //    // Validate the JWT Issuer (iss) claim
        //    //    ValidateIssuer = true,
        //    //    ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
        //    //    // Validate the JWT Audience (aud) claim
        //    //    ValidateAudience = true,
        //    //    ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
        //    //    //Validate the token expiry
        //    //    ValidateLifetime = true,
        //    //    //If you want to allow a certain amount of clock drift, set that here:
        //    //    ClockSkew = TimeSpan.Zero
        //    //};

        //    //app.UseJwtBearerAuthentication(new JwtBearerOptions
        //    //{
        //    //    AutomaticAuthenticate = true,
        //    //    AutomaticChallenge = true, 
        //    //    TokenValidationParameters = tokenValidationParameters
        //    //});

        //    //app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    //{
        //    //    AutomaticAuthenticate = true,
        //    //    AutomaticChallenge = true,
        //    //    AuthenticationScheme = "Cookie",
        //    //    CookieName = Configuration.GetSection("TokenAuthentication:CookieName").Value,
        //    //    TicketDataFormat = new CustomJwtDataFormat(
        //    //        SecurityAlgorithms.HmacSha256,
        //    //        tokenValidationParameters)

        //    //});
        //}
    }
}
