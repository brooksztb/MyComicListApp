using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using MyComicListApi.Models;
using MyComicListApi.CustomTokenAuthProvider;
using MyComicListApi.Services;
using Microsoft.AspNetCore.Http;
using MyComicListApi.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//Initial Commit
namespace MyComicListApi.Controllers
{
    [Route("[controller]")]
    public class JwtController : Controller
    {
        private readonly TokenProviderOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        public IApplicationUserRepository AppUser { get; set; }

        public JwtController(IOptions<TokenProviderOptions> jwtOptions, ILoggerFactory loggerFactory, IApplicationUserRepository user)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<JwtController>();
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            AppUser = user;
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromForm] ApplicationUser applicationUser)
        {
            var identity = await GetClaimsIdentity(applicationUser);
            if(identity == null)
            {
                _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                return BadRequest("Invalid Credentials");
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("ActiveUser")
            };

            //Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                 claims: claims,
                 expires: _jwtOptions.Expiration,
                 signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] ApplicationUser newUser)
        {
            try
            {
                var encryptedPassword = Hasher.ComputeSHA256(newUser.Password);
                string result = await AppUser.RegisterNewUser(newUser.UserName, encryptedPassword, newUser.Email, newUser.FirstName, newUser.LastName);

                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if(options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentNullException("Must be a non-zero Timespan.", nameof(TokenProviderOptions.ValidFor));
            }

            if(options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if(options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }

        ///<return>Date converted to seconds since Unix Epoch (Jan 1, 1970, midnight UTC)</return>
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private Task<ClaimsIdentity> GetClaimsIdentity(ApplicationUser user)
        {
            bool isValidUser = AppUser.ValidateCredentials(user.UserName, user.Password);

            if (isValidUser)
            {
                return Task.FromResult(new ClaimsIdentity(
                    new GenericIdentity(user.UserName, "Token"),
                    new[]
                    {
                        new Claim("Activeuser", user.UserName)
                    }));
            }

            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
