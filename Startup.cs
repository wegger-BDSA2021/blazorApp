using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using blazorApp.Data;
using Microsoft.Graph;
using Services;
using System.Net.Http;

namespace blazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var initialScopes = Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
            // var customScopes = Configuration.GetValue<string>("WeggerAuth:WeggerScope");
            // initialScopes.ToList().Add(customScopes);

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"))
                    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                        .AddMicrosoftGraph(Configuration.GetSection("DownstreamApi"))
                            // .AddDownstreamWebApi("WeggerApi", Configuration.GetSection("WeggerAuth"))
                        .AddInMemoryTokenCaches();

            // services.AddMicrosoftWebAppAuthentication(Configuration)
            //     .AddMicrosoftWebAppCallsWebApi(Configuration, new List<string>()
            //     {"api://23d9a97d-3387-499c-83bc-0ae84d198403/ReadAccess"})
            //     .AddInMemoryTokenCaches();


            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));

            }).AddMicrosoftIdentityUI();

            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
                options.AddPolicy("ValidateAccessTokenPolicy", validateAccessTokenPolicy =>
                    {
                        // Validate ClientId from token
                        // only accept tokens issued ....
                        validateAccessTokenPolicy.RequireClaim("ReadAccess", "23d9a97d-3387-499c-83bc-0ae84d198403");
                    });
            });

            services.AddRazorPages();

            services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            services.AddSingleton<WeatherForecastService>();

            services.AddHttpClient<IResourceService, ResourceService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            }).ConfigureHttpMessageHandlerBuilder(builder =>
            {
                builder.PrimaryHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
