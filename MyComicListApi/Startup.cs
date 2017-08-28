using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyComicListApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyComicListApi.CustomTokenAuthProvider;

namespace MyComicListApi
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        private string SecretKey;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<PullListDbSettings>(Configuration.GetSection("DocumentDbSettings"));
            services.Configure<ShortBoxedSettings>(Configuration.GetSection("ShortBoxedApiSettings"));
            // Add framework services.
            services.AddMvc();
            services.AddScoped<IPullListRepository, PullListRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IShortBoxedRepository, ShortBoxedRepository>();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(TokenProviderOptions));
            SecretKey = Configuration.GetSection("AppSettings").GetValue<string>("SecretKey");
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            services.Configure<TokenProviderOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(TokenProviderOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(TokenProviderOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //ConfigureAuth(app);

            app.UseMvc();
        }
    }
}
