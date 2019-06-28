using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using HR.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using HR.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using HR.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using System.IO;

namespace HR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(o => {
                o.SuppressXFrameOptionsHeader = true;
                o.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddDataProtection();
            var protectionProvider = DataProtectionProvider.Create("MyApplication");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AnyCookie", policy =>
                {
                    policy.AuthenticationSchemes.Add("ARM");
                    policy.AuthenticationSchemes.Add(AzureADDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = AzureADDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>(ApiAuthDefaults.AuthenticationScheme, null)
            .AddCookie("ARM", options =>
            {
                options.Cookie.Name = "AMAHosted.Token";
                options.TicketDataFormat = new TicketDataFormat(protectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "ARM", "v2"));
            })
            .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddScoped<IApiUserService, ApiUserService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient(EmployeeFunctionClient.DefaultClientName);
            services.AddHttpClient(EmployeeFunctionsController.ControllerClientName);

            services.AddMvc()
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/Index", "AnyCookie");
                options.Conventions.AuthorizePage("/Tools", "AnyCookie");
                options.Conventions.AuthorizeFolder("/Employees", "AnyCookie");
                options.Conventions.AuthorizeFolder("/EmployeeFunctions", "AnyCookie");
                options.Conventions.AllowAnonymousToPage("/AMAHosted");
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
