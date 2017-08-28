using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MyComicListApi.CustomTokenAuthProvider
{
    public class TokenProviderOptions
    {
        /// <summary>
        /// The relative request path to listen one.
        /// </summary>
        /// <remarks>The default path is <c>token</c></remarks>
        public string Path { get; set; } = "/token";

        /// <summary>
        /// The Issuer (iss) claim for generated tokens
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The Audience (aud) claim for generated tokens.
        /// </summary>
        public string Audience { get; set; }

        ///<summary>
        /// "iat" (Issued At) Claim (default is UTC NOW)
        ///</summary>
        /// <remarks>
        /// The iat claim indentifies the time at which the JWT was issued. The can be used to determine the age of the JWT.
        /// </remarks>
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Set the Timespan the token will be valid for (default is five minutes (300 seconds))
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The expiration time for the generated tokens
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Resolves a user identity given a username and password.
        /// </summary>
        public Func<string, string, Task<ClaimsIdentity>> IdentityResolver { get; set; }

        /// <summary>
        /// Generates a random value (nonce) for each generated token
        /// </summary>
        /// <remarks>The default nonce is a random GUID.</remarks>
        public Func<Task<string>> NonceGenerator { get; set; }
            = () => Task.FromResult(Guid.NewGuid().ToString());
    }
}
